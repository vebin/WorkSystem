namespace YmtSystem.Domain.Event
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;

    public class EventSourceRootEntity:Entity<string>
    {
        public string EventId { get; set; }
        public string EventTypeFullName { get; set; }
        public byte[] Body { get; set; }
        public int Version { get; set; }

        public IDomainEvent ToDomainEvent() 
        {
            var type = Type.GetType(this.EventTypeFullName);
            return null;
        }
    }
}
