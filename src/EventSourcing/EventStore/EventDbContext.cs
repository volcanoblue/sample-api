using Microsoft.EntityFrameworkCore;
using Moonad;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VolcanoBlue.Core.Error;
using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.EventSourcing.EventStore
{
    public sealed class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
    {
        private DbSet<Event> Events { get; set; }

        public async Task<Result<ImmutableArray<IEvent>, IError>> ReadStreamAsync(string streamId, 
                                                                                  CancellationToken ct = default)
        {
            try
            {
                var stream = await Events.AsNoTracking()
                                         .Where(e => e.StreamId == streamId)
                                         .ToArrayAsync(ct);

                if (stream is not null && stream.Length > 0)
                    return stream.OrderBy(e => e.Id)
                                 .Select(EventSerializer.Desserialize)
                                 .ToImmutableArray();

                return ImmutableArray<IEvent>.Empty;
            }
            catch(Exception exc)
            {
                return new ExceptionalError(exc);
            }
        }

        public async Task<Result<ImmutableArray<IEvent>, IError>> ReadStreamFromEventIdAsync(string streamId, long eventId, 
                                                                                          CancellationToken ct = default)
        {
            try
            {
                var stream = await Events.AsNoTracking()
                                         .Where(e => e.StreamId == streamId && e.Id > eventId)
                                         .ToArrayAsync(ct);

                if(stream is not null && stream.Length > 0)
                     return stream.OrderBy(e => e.Id)
                                  .Select(EventSerializer.Desserialize)
                                  .ToImmutableArray();

                return ImmutableArray<IEvent>.Empty;
            }
            catch(Exception exc)
            {
                return new ExceptionalError(exc);
            }
        }

        public async Task AppendToStreamAsync(string streamId, IEnumerable<IEvent> events, CancellationToken ct = default)
        {
            var eventsToAppend = events.Select(e => EventSerializer.Serialize(streamId, e));
            await Events.AddRangeAsync(eventsToAppend, ct);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasKey(e => new { e.Id, e.StreamId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
