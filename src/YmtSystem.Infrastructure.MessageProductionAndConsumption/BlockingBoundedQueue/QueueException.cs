
namespace YmtSystem.Infrastructure.MPAC.BlockingBoundedQueue
{
    using System;
    using System.Runtime.Serialization;   

    public class QueueException : Exception
    {
        public QueueException() : base() { }
        
        public QueueException(string msg) : base(msg) { }

        public QueueException(string msg,Exception ie) : base(msg,ie) { }

        public QueueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
