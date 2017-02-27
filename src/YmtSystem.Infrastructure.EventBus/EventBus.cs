namespace YmtSystem.Infrastructure.EventBusService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using YmtSystem.CrossCutting;
    using System.Threading;

    /// <summary>
    ///  Local Memory EventBus
    /// </summary>
    public class EventBus
    {
        //pool
        private static readonly ThreadLocal<EventBus> pool = new ThreadLocal<EventBus>();
        //事件处理器
        private static Dictionary<Type, HashSet<dynamic>> eventHandler = new Dictionary<Type, HashSet<dynamic>>();
        private EventBus() { }
        //[ThreadStatic]
        //private static EventBus3 eb;
        public static EventBus Instance
        {
            get
            {
                if (pool.Value == null)
                    pool.Value = new EventBus();
                return pool.Value;
            }
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var _type = typeof(TEvent);
            foreach (var _handler in eventHandler)
            {
                if (_handler.Key == _type)
                {
                    _handler.Value.Each(__handler =>
                    {
                        var _h = __handler as IEventSubscriber<TEvent>;
                        if (_h.Strategy.AsyncReceive)
                        {
                            AsyncProcessEvent<TEvent>(@event, _h);
                        }
                        else
                        {
                            SyncProcessEvent<TEvent>(@event, _h);
                        }
                    });
                }
            }
        }

        public void Publish<TEvent>(IEnumerable<TEvent> @event) where TEvent : IEvent
        {
            @event.Each(e => this.Publish(e));
        }

        public void Subscribe<TEvent>(IEventSubscriber<TEvent> handler)
            where TEvent : IEvent
        {
            var _type = typeof(TEvent);
            if (!eventHandler.ContainsKey(_type))
            {
                var _handlerList = new HashSet<dynamic>();
                _handlerList.Add(handler);
                eventHandler[_type] = _handlerList;
            }
            else
            {
                var _handlerList = eventHandler[_type];
                if (!_handlerList.Contains(handler))
                    eventHandler[_type].Add(handler);
            }
        }

        public void UnSubscribe<TEvent>() where TEvent : IEvent
        {
            eventHandler.Remove(typeof(TEvent));
        }

        public void Clear()
        {
            pool.Dispose();
            eventHandler.Clear();
        }

        private void SyncProcessEvent<TEvent>(TEvent @event, IEventSubscriber<TEvent> _h) where TEvent : IEvent
        {
            var _retry = _h.Strategy.Retry;
            var _fail = false;
            do
            {
                try
                {
                    _fail = false;
                    _h.Handle(@event);
                }
                catch (Exception ex)
                {
                    _fail = true;
                    _h.Error(ex);
                }
            }
            while (Interlocked.Decrement(ref _retry) > 0 && _fail == true);
            if (_fail && _h.Strategy.PersistentStore.HasValue && _h.Strategy.PersistentStore.Value)
            {
                //TODO:持久化存储，处理失败的事件
            }
        }

        private void AsyncProcessEvent<TEvent>(TEvent @event, IEventSubscriber<TEvent> _h)
            where TEvent : IEvent
        {
            var _retry = _h.Strategy.Retry;
            var _fail = false;
            do
            {
                try
                {
                    _fail = false;
                    if (_h.Strategy.TimeOut.HasValue)
                        Task.Factory.StartNew(() => _h.Handle(@event)).WithTimeout(_h.Strategy.TimeOut.Value);
                    else
                        Task.Factory.StartNew(() => _h.Handle(@event));
                }
                catch (AggregateException ex)
                {
                    _fail = true;
                    ex.Handle(e =>
                                {
                                    _h.Error(ex);
                                    return true;
                                });
                }
            }
            while (Interlocked.Decrement(ref _retry) > 0 && _fail == true);
            if (_fail && _h.Strategy.PersistentStore.HasValue && _h.Strategy.PersistentStore.Value)
            {
                //TODO:持久化存储，处理失败的事件
            }
        }
    }
}
