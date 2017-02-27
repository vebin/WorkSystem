namespace YmtSystem.Infrastructure.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EventSourceRootEntity
    {
        public string EventId { get; set; }
        public string EventTypeFullName { get; set; }
        public byte[] Body { get; set; }
        public DateTime CreateTime { get; set; }
        
    }
}
