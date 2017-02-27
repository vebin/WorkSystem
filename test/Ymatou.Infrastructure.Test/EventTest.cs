using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.Domain.Event;
using YmtSystem.Infrastructure.EventBusService;



namespace YmtSystem.Infrastructure.Test
{
    [TestFixture]
    public class EventTest
    {
        [Test]
        public void EventBus3Test()
        {
            EventBus.Instance.Subscribe(new AEventSubscriber());
            EventBus.Instance.Subscribe(new A2EventSubscriber());
            EventBus.Instance.Publish(new AEvent { A = 1000 });
        }
    }
    public class A2EventSubscriber : IDomainEventSubscriber<AEvent>
    {
        public EventHandlerStrategy Strategy
        {
            get
            {
                return new EventHandlerStrategy
                {
                    AsyncReceive = true
                };
            }
        }

        public void Handle(AEvent @event)
        {
            Console.WriteLine("处理AEvent 2 ,{0}", @event.A);
        }

        public void Start()
        {
            Console.WriteLine("初始化资源");
        }

        public void Complete()
        {
            Console.WriteLine("end...");
        }

        public void Error(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    public class AEventSubscriber : DomainEventSubscriber<AEvent>
    {      
        public override void Handle(AEvent @event)
        {
            Console.WriteLine("处理AEvent 1 ,{0}", @event.A);
        }
    }

    public class CEventHandler2 : IEventSubscriber<IEvent>
    {
        public void Handle(IEvent @event)
        {
            var c = @event as CEvent;
            Console.WriteLine("处理中。。。{0}", c.A);
        }
        public EventHandlerStrategy Strategy
        {
            get
            {
                return new EventHandlerStrategy
                    {
                        AsyncReceive = true
                    };
            }
        }


        public void Start()
        {

        }

        public void Complete()
        {

        }

        public void Error(Exception ex)
        {

        }
    }
    public class CEventHandler : DomainEventSubscriber<CEvent>
    {
        public override void Handle(CEvent @event)
        {
            var c = @event as CEvent;
            Console.WriteLine("处理中。。。{0},,,thID，，{1}", c.A, Thread.CurrentThread.ManagedThreadId);
        }
        public override EventHandlerStrategy Strategy
        {
            get
            {
                return new EventHandlerStrategy
                    {
                        AsyncReceive = true
                    };
            }
        }

    }
    public class CEvent : DomainEvent
    {
        public int A { get; set; }

    }
    public class AEvent : DomainEvent
    {
        public int A { get; set; }

    }
    public class BEvent : DomainEvent
    {
        public int B { get; set; }

    }

}
