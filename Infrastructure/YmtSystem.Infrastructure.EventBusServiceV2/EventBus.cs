using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace YmtSystem.Infrastructure.EventBusServiceV2
{
    public sealed class EventBus
    {
        //pool
        private static readonly ThreadLocal<EventBus> pool = new ThreadLocal<EventBus>();
        //事件处理器
        private static Dictionary<Type, HashSet<dynamic>> eventHandler = new Dictionary<Type, HashSet<dynamic>>();
        private BufferBlock<dynamic> buffer;
        private ActionBlock<dynamic> consumer;
        private EventBus() { }
        public static EventBus Instance
        {
            get
            {
                if (pool.Value == null)
                    pool.Value = new EventBus();
                return pool.Value;
            }
        }
        public void Publish<TEvent>(TEvent @event, TimeSpan timeOut, Action<Exception> errorHander = null) where TEvent : IEvent
        {
            HashSet<dynamic> _handlers;
            if (!eventHandler.TryGetValue(typeof(TEvent), out _handlers))
                throw new KeyNotFoundException(string.Format("事件{0}处理器不存在", typeof(TEvent)));
            var cancel = new CancellationTokenSource(timeOut);
            var token = cancel.Token;

            buffer = new BufferBlock<dynamic>(new DataflowBlockOptions { BoundedCapacity = Environment.ProcessorCount });
            consumer = new ActionBlock<dynamic>(e =>
            {
                _handlers.Each(_h =>
                {
                    var subscriber = _h as IEventSubscriber<TEvent>;
                    if (subscriber == null)
                        throw new InvalidOperationException(string.Format("{0}无法转换成事件订阅器", _h.GetType()));
                    var _retry = subscriber.Strategy.Retry;
                    var _fail = false;
                    do
                    {
                        try
                        {
                            if (token.IsCancellationRequested)
                                token.ThrowIfCancellationRequested();
                            subscriber.Handle(@event);
                            _fail = false;
                        }
                        catch (OperationCanceledException ex) 
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            subscriber.Error(ex);
                            _fail = true;
                        }
                    } while (_fail && Interlocked.Decrement(ref _retry) > 0);
                });
            }, new ExecutionDataflowBlockOptions { CancellationToken = token, MaxDegreeOfParallelism = Environment.ProcessorCount });

            buffer.SendAsync(@event, token);
            buffer.LinkTo(consumer);
            try
            {
                buffer.Complete();
                buffer.Completion.Wait(token);

                consumer.Complete();
                consumer.Completion.Wait(token);
            }
            catch (OperationCanceledException ex)
            {
                if (errorHander != null)
                    errorHander(ex);
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
