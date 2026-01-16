using Mvc = Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;

namespace VolcanoBlue.SampleApi.Infrastructure.ProblemDetails
{
    /// <summary>
    /// [INFRASTRUCTURE - OBJECT POOLING] Policy for pooling ProblemDetails instances.
    /// Architectural Role: Reduces GC pressure by reusing ProblemDetails objects in high-throughput scenarios.
    /// Defines creation and cleanup logic for pooled objects.
    /// </summary>
    public sealed class ProblemDetailsPooledObjectPolicy : PooledObjectPolicy<Mvc.ProblemDetails>
    {
        public override Mvc.ProblemDetails Create() => new();

        public override bool Return(Mvc.ProblemDetails obj)
        {
            // Reset all properties before returning to pool
            obj.Status = null;
            obj.Title = null;
            obj.Detail = null;
            obj.Instance = null;
            obj.Type = null;
            obj.Extensions?.Clear();

            return true; // Always return to pool
        }
    }
}
