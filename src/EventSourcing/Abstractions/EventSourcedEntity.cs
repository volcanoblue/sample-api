using System.Collections.Generic;

namespace VolcanoBlue.EventSourcing.Abstractions
{
    /// <summary>
    /// [INFRASTRUCTURE - EVENT SOURCING BASE] Base class for entities using Event Sourcing.
    /// Architectural Role: Provides infrastructure for applying events and managing entity version.
    /// Maintains collection of uncommitted events that need to be persisted to the event store.
    /// Implements the event application pattern through abstract ProcessEvent method.
    /// Derived classes must implement ProcessEvent to handle their specific domain events.
    /// </summary>
    public abstract class EventSourcedEntity
    {
        public long Version { get; protected set; } = 1;
        public List<IEvent> UncommittedEvents { get; } = [];
        
        protected void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Version++;
            UncommittedEvents.Add(@event);
            ProcessEvent(@event);
        }

        protected void FeedEvents(IEnumerable<IEvent> stream)
        {
            foreach (var @event in stream)
            {
                Version = @event.EventId;
                ProcessEvent(@event);
            }

            Version++;
        }

        protected abstract void ProcessEvent(IEvent @event);
    }
}
