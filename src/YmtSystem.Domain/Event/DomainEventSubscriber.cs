namespace YmtSystem.Domain.Event
{
    using System;
    using YmtSystem.Infrastructure.EventBusService;

    public abstract class DomainEventSubscriber<TEvent> : IEventSubscriber<TEvent> where TEvent : IDomainEvent
    {
        public abstract void Handle(TEvent @event);

        public virtual EventHandlerStrategy Strategy
        {
            get
            {
                return new EventHandlerStrategy
                            {
                                AsyncReceive = false,                               
                                PersistentStore = false,
                                Retry = 1,
                            };
            }
        }
    
        public virtual void Error(Exception ex)
        {
            //TODO           
        }
    }
}
