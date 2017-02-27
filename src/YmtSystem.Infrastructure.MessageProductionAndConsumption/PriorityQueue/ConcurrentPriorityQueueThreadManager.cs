

namespace YmtSystem.Infrastructure.MPAC.PQ
{
    using System;
    using System.Collections.Generic;
    using System.Linq; 
    using System.Text;
    using System.Threading;
    using YmtSystem.Infrastructure.MPAC.BlockingBoundedQueue;
    

    public sealed class ConcurrentPriorityQueueThreadManager<K, V> where K : IComparable<K>
    {
        private ConcurrentPriorityQueue<K, V> m_queue = new ConcurrentPriorityQueue<K, V>();
        private int m_capacity;
        private object m_fullEvent = new object();
        private int m_fullWaiters = 0;
        private object m_emptyEvent = new object();
        private int m_emptyWaiters = 0;
        //新增策略：当生产数据到达上限时等待消费的超时时间，如果过了这个时间还没有消费者到来，则删除老的数据，让新数据入列
        private TimeSpan _waitConsumerTimeOut = new TimeSpan(0, 0, 3);
        //新增策略：当没有生产数据时，消费者等待超时时间，如果过了这个时间还没有数据生产，则释放当前锁,并通知生产者生产数据
        private TimeSpan _watiProduceTimeOut = new TimeSpan(0, 0, 2);

        public ConcurrentPriorityQueueThreadManager(int capacity = 100000)
        {
            if (capacity <= 0 || capacity > int.MaxValue)
                throw new QueueException("参数capacity无效");
            m_capacity = capacity;            
        }

        public int Count
        {
            get
            {
                lock (m_queue)
                    return m_queue.Count;
            }
        }

        public void Clear()
        {
            lock (m_queue)
                m_queue.Clear();
        }

        public bool Contains(KeyValuePair<K,V> item)
        {
            lock (m_queue)
                return m_queue.Contains(item);
        }

        public void Enqueue(K k,V v)
        {
            lock (m_queue)
            {
                //队列到达上限，则等待一个消费者到来
                // If full, wait until an item is consumed.                
                while (m_queue.Count == m_capacity)
                {                  
                    m_fullWaiters++;
                    try
                    {
                        lock (m_fullEvent)
                        {
                            Monitor.Exit(m_queue);
                            //Monitor.Wait(m_fullEvent); // @BUGBUG: deadlock prone.
                            //策略改变：
                            Monitor.Wait(m_fullEvent, _waitConsumerTimeOut);
                            Monitor.Enter(m_queue);                            
                        }
                    }
                    finally
                    { 
#if DEBUG
                        Console.WriteLine("队列大小：{0},{1},{2}", m_queue.Count, m_capacity, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
#endif
                        while (m_queue.Count == m_capacity)
                        {
                            //Console.WriteLine("死循环");
#if DEBUGs                 
                            
                            Console.WriteLine("删除老数据：{0},{1}", m_queue.Dequeue(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));                          
#else
                            m_queue.TryDequeue();
#endif
                        }
                        m_fullWaiters--;
                    }
                }
#if DEBUG
                Console.WriteLine("新数据：{0}入列,{1}", k, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
#endif
                m_queue.Enqueue(k, v);
            }

            //至少有一个消费者则唤醒消费者
            // Wake consumers who are waiting for a new item.
            if (m_emptyWaiters > 0)
                lock (m_emptyEvent)
                    Monitor.Pulse(m_emptyEvent);
        }

        public KeyValuePair<K,V> Dequeue()
        {
            KeyValuePair<K, V> item;

            lock (m_queue)
            {
                while (m_queue.Count == 0)
                {
                    // Queue is empty, wait for a new item to arrive.
                    //等待消费的数量
                    m_emptyWaiters++;
                    try
                    {
                        lock (m_emptyEvent)
                        {
                            Monitor.Exit(m_queue);
                            //Monitor.Wait(m_emptyEvent); // @BUGBUG: deadlock prone.
                            //策略改变：
                            Monitor.Wait(m_emptyEvent, _watiProduceTimeOut);
                            Monitor.Enter(m_queue);
                        }
                    }
                    finally
                    {
                        m_emptyWaiters--;
                    }
                }

                item = m_queue.TryDequeue();
            }

            //至少有一个生产者则唤醒生产者
            // Wake producers who are waiting to produce.
            if (m_fullWaiters > 0)
                lock (m_fullEvent)
                    Monitor.Pulse(m_fullEvent);

            return item;
        }

        public KeyValuePair<K, V> Peek()
        {
            lock (m_queue)
                return m_queue.TryPeek();
        }
    }
}
