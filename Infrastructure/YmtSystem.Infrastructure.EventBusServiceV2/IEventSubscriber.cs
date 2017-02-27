namespace YmtSystem.Infrastructure.EventBusServiceV2
{
    using System;

    public interface IEventSubscriber<TEvent> where TEvent : IEvent
    {
        void Handle(TEvent domainEvent);      
        void Error(Exception ex);
        EventHandlerStrategy Strategy { get; }
    }  
}
