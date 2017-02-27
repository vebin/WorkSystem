using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YmtSystem.Infrastructure.EventBusServiceV2;

namespace YmtSystem.Infrastructure.EventBus2.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            EventBus.Instance.Subscribe(new AEventHandler());

            EventBus.Instance.Publish(new AEvent { A = 1 }, TimeSpan.FromSeconds(1), errorHander: ex => Console.WriteLine(ex.ToString()));
            EventBus.Instance.Publish(new AEvent { A = 2 }, TimeSpan.FromSeconds(1), errorHander: ex => Console.WriteLine(ex.ToString()));
            EventBus.Instance.Publish(new AEvent { A = 3 }, TimeSpan.FromSeconds(1), errorHander: ex => Console.WriteLine(ex.ToString()));

            Console.Read();
        }
    }
    public class AEventHandler : IEventSubscriber<AEvent>
    {

        public void Handle(AEvent domainEvent)
        {
            Console.WriteLine("事件" + domainEvent.A + "处理了 ，线程ID" + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(TimeSpan.FromSeconds(3));
        }

        public void Error(Exception ex)
        {
            //Console.WriteLine(ex.ToString());

        }

        public EventHandlerStrategy Strategy
        {
            get { return new EventHandlerStrategy { AsyncReceive = true, Retry = 0 }; }
        }
    }
    public class AEvent : IEvent
    {
        public int A { get; set; }
    }
}
