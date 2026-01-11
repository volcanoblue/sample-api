using System;
using System.Collections.Generic;
using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.EventSourcing
{
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
