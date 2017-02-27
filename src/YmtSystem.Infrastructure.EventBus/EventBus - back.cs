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

    public class EventBus
    {
        private ConcurrentDictionary<string, ConcurrentQueue<dynamic>> eventContainer = new ConcurrentDictionary<string, ConcurrentQueue<dynamic>>();
        private ConcurrentDictionary<string, dynamic> handlerContainer = new ConcurrentDictionary<string, dynamic>();
        private static readonly Lazy<EventBus> lazy = new Lazy<EventBus>(() => new EventBus(), true);
        public static EventBus Instance
        {
            get { return lazy.Value; }
        }
        private EventBus()
        {
        }
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var _eventName = typeof(TEvent).FullName;
            EnsureEventContainer(_eventName);
            PbulishEvent(@event, _eventName);
            ConsumerEvent();
        }
        public void Publish<TEvent>(IEnumerable<TEvent> @event) where TEvent : IEvent
        {
            var _eventName = typeof(TEvent).FullName;
            foreach (var _e in @event)
            {
                EnsureEventContainer(_eventName);
                PbulishEvent(_e, _eventName);
            }
            ConsumerEvent();
        }
        public void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            var _eventFullNmae = typeof(TEvent).FullName;
            if (!handlerContainer.ContainsKey(_eventFullNmae))
                handlerContainer[_eventFullNmae] = handler;
        }
        public void UnSubscribe<TEvent>() where TEvent : IEvent
        {
            var _eventFullNmae = typeof(TEvent).FullName;
            if (handlerContainer.ContainsKey(_eventFullNmae))
            {
                object o;
                handlerContainer.TryRemove(_eventFullNmae, out o);
                if (o != null)
                {
                    ConsumerEvent();
                }
            }
        }
        private void ConsumerEvent()
        {
            foreach (var handler in handlerContainer)
            {
                ConcurrentQueue<dynamic> eq;
                if (eventContainer.TryGetValue(handler.Key, out eq))
                {
                    while (eq.Any())
                    {
                        dynamic o;
                        if (eq.TryDequeue(out o))
                        {
                            try
                            {
                                handler.Value.Handle(o);
                            }
                            catch (Exception ex)
                            {
                                YmtSystem.CrossCutting.YmatouLoggingService.Error("event handler error {0}", ex.ToString());
                            }
                        }
                    }
                }
            }
        }
        private void PbulishEvent(object @event, string @eventFullName)
        {
            eventContainer[@eventFullName].Enqueue(@event);
        }
        private void EnsureEventContainer(string @eventFullName)
        {
            if (!eventContainer.ContainsKey(@eventFullName))
            {
                eventContainer[@eventFullName] = new ConcurrentQueue<dynamic>();
            }
        }
    }

    /// <summary>
    ///  Local Memory EventBus
    /// </summary>
    public class EventBus3
    {   
        //pool
        private static readonly ThreadLocal<EventBus3> pool = new ThreadLocal<EventBus3>();
        //事件处理器
        private Dictionary<Type, HashSet<dynamic>> eventHandler = new Dictionary<Type, HashSet<dynamic>>();
        private EventBus3() { }
        //[ThreadStatic]
        //private static EventBus3 eb;
        public static EventBus3 Instance
        {
            get
            {
                if (pool.Value == null)
                    pool.Value = new EventBus3();
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
                            Task.Factory.StartNew(() => _h.Handle(@event));
                        }
                        else
                        {
                            __handler.Handle(@event);
                        }
                    });
                }
            }
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
    }
}
