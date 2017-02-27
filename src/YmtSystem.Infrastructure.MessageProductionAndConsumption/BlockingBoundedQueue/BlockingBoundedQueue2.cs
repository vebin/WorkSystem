namespace YmtSystem.Infrastructure.MPAC.BlockingBoundedQueue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;

    public sealed class __BlockingBoundedQueue<T>
    {
        private Queue<T> m_queue = new Queue<T>();
        private object empty_lock = new object();
        private object full_lock = new object();
        private int wait_dequeue;

        public void Enqueue(T item)
        {
            lock (m_queue)
                m_queue.Enqueue(item);
            if (wait_dequeue > 0)
                lock (empty_lock)
                    Monitor.Pulse(empty_lock);
        }

        public T Dequeue()
        {
            lock (m_queue)
            {
                while (m_queue.Count == 0)
                {
                    wait_dequeue++;
                    lock (empty_lock)
                    {
                        Monitor.Exit(m_queue);
                        Monitor.Wait(empty_lock);
                        Monitor.Enter(m_queue);
                    }
                }
                wait_dequeue = 0;
                var item = m_queue.Dequeue();
                return item;
            }
        }

        public void Handle(Action<T> handle)
        {
            new Thread(() =>
            {
                while (true)
                {
                    var item = Dequeue();
                    handle(item);
                }
            }).Start();
        }
    }
}
