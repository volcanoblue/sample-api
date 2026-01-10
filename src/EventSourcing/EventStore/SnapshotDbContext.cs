using Microsoft.EntityFrameworkCore;
using Moonad;
using System;
using System.Threading;
using System.Threading.Tasks;
using VolcanoBlue.Core.Error;
using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.EventSourcing.EventStore
{
    public sealed class SnapshotDbContext(DbContextOptions<SnapshotDbContext> options) : DbContext(options)
    {
        private DbSet<Snapshot> Snapshots { get; set; }

        public async Task<Result<TResult, IError>> ByIdAsync<TResult>(string snapshotId, 
                                                                      CancellationToken cancellationToken = default)
            where TResult : notnull, ISnapshot
        {
            try
            {
                var snapshot = (await Snapshots.AsNoTracking()
                                               .FirstOrDefaultAsync(e => e.EntityId == snapshotId, cancellationToken))
                                               .ToOption();
                if (snapshot)
                    return SnapshotSerializer.Deserialize<TResult>(snapshot.Get());

                return SnapshotNotFoundError.Instance;
            }
            catch (Exception exc)
            {
                return new ExceptionalError(exc);
            }
        }

        public async Task UpsertAsync<T>(string entityId, T snapshot, CancellationToken ct = default) 
            where T : ISnapshot
        {
            var storedSnapshot = (await Snapshots.AsNoTracking()
                                                 .FirstOrDefaultAsync(s => s.EntityId == entityId, ct))
                                                 .ToOption();
            if (storedSnapshot)
                Remove(storedSnapshot.Get());

            await Snapshots.AddAsync(SnapshotSerializer.Serialize(entityId, snapshot), ct);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Snapshot>().HasKey(e => new { e.EntityId, e.EntityVersion });
            base.OnModelCreating(modelBuilder);
        }
    }
}
