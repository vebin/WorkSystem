namespace YmtSystem.Infrastructure.EventBusService
{
    using System;

    public interface IEventSubscriber<TEvent> where TEvent : IEvent
    {
        void Handle(TEvent domainEvent);
      
        void Error(Exception ex);
        EventHandlerStrategy Strategy { get; }
    }  
}
