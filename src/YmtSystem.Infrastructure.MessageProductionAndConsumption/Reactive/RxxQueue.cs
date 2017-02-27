namespace YmtSystem.Infrastructure.MPAC.Reactive
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Reactive.Linq;
    using Rxx.Parsers.Linq;
    using Rxx.Parsers.Reactive.Linq;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Threading;
    using YmtSystem.CrossCutting;

    /// <summary>
    /// 反应式生产消费队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RxxQueue<T>
    {
        private static readonly Lazy<RxxQueue<T>> lazy = new Lazy<RxxQueue<T>>(() => new RxxQueue<T>(), true);

        private ConcurrentQueue<T> queue;

        private RxxQueue()
        {
            queue = new ConcurrentQueue<T>();
        }

        public static RxxQueue<T> Instance
        {
            get { return lazy.Value; }
        }

        public RxxQueue<T> Production(T val)
        {
            queue.Enqueue(val);
            return this;
        }
        public RxxQueue<T> Production(IEnumerable<T> val)
        {
            val.Each(e => queue.Enqueue(e));
            return this;
        }

        public RxxQueue<T> Consumption(TimeSpan timeOut, Action<T> handle, Action<Exception> errorHandle = null, Action end = null)
        {         
            queue
                 .ToObservable(Scheduler.ThreadPool)
                 .Timeout(timeOut)
                 .Subscribe(next =>
                 {
                     handle.ToAsync()(next);
                 }, err =>
                 {
                     errorHandle(err);
                 }
                 , () =>
                 {
                     end();
                 }
                 );
            return this;
        }
    }
}
