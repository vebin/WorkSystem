

namespace YmtSystem.Infrastructure.MPAC.PQ
{
    using System;

    using BlockingBoundedQueue;

    public class PQValue<K, V> where K : IComparable<K>
    {
        public V Value { get; set; }
        public K Key { get; set; }
        public QueueItemConsumerStats ConsumerResult { get; set; }
        internal int ConsumerCount { get; set; }
    }
}
