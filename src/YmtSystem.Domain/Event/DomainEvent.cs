

namespace YmtSystem.Domain.Event
{
    using System;

    public abstract class DomainEvent : IDomainEvent
    {
        public string EventKey { get; set; }
        public DateTime TriggerTime { get; set; }

        public DomainEvent(string eventKey)
        {
            this.EventKey = eventKey;
            this.TriggerTime = DateTime.Now;
        }

        public DomainEvent()
            : this(Guid.NewGuid().ToString("N"))
        {

        }
    }
}
