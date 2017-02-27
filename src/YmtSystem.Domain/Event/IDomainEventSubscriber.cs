namespace YmtSystem.Domain.Event
{
    using System;
    using YmtSystem.Infrastructure.EventBusService;

    public interface IDomainEventSubscriber<TEvent> : IEventSubscriber<TEvent> where TEvent : IDomainEvent
    {

    }
}
