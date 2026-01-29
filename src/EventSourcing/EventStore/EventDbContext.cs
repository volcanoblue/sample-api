using Microsoft.EntityFrameworkCore;
using Moonad;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using VolcanoBlue.Core.Error;
using VolcanoBlue.EventSourcing.Abstractions;
using VolcanoBlue.EventSourcing.EventStore.Serialization;

namespace VolcanoBlue.EventSourcing.EventStore
{
    /// <summary>
    /// [INFRASTRUCTURE - EVENT STORE] Entity Framework DbContext for event persistence.
    /// Architectural Role: Implements append-only event store using Entity Framework Core.
    /// Provides operations to read event streams and append new events to streams.
    /// Each stream is identified by a streamId (typically aggregate ID prefixed with type).
    /// Supports reading full streams or from a specific event version for incremental updates.
    /// Returns immutable arrays to prevent accidental modifications to event history.
    /// </summary>
    public sealed class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
    {
        private DbSet<Event> Events { get; set; }

        public async Task<Result<ImmutableArray<IEvent>, IError>> ReadStreamAsync(string streamId, 
                                                                                  CancellationToken ct = default)
        {
            try
            {
                var stream = Events.AsNoTracking()
                                   .Where(e => e.StreamId == streamId)
                                   .OrderBy(e => e.Id);

                var count = await stream.CountAsync(ct);
                if(count == 0)
                    return ImmutableArray<IEvent>.Empty;

                var builder = ImmutableArray.CreateBuilder<IEvent>(count);
                builder.AddRange(stream.Select(EventSerializer.Deserialize));
                return builder.ToImmutable();
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
                                  .Select(EventSerializer.Deserialize)
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

        public async Task<Result<int, IError>> SaveStreamAsync(CancellationToken ct = default)
        {
            try
            {
                return await SaveChangesAsync(ct);
            }
            catch(Exception exc)
            {
                return await Task.FromResult(new ExceptionalError(exc));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasKey(e => new { e.Id, e.StreamId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
