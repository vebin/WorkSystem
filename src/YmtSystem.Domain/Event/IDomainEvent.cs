namespace YmtSystem.Domain.Event
{
    using System;
    using YmtSystem.Domain.Shard;
    using YmtSystem.Infrastructure.EventBusService;
    /// <summary>
    /// 领域事件
    /// </summary>
    public interface IDomainEvent : IEvent
    {
        /// <summary>
        /// 事件唯一标识
        /// </summary>
        string EventKey { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        DateTime TriggerTime { get; set; }
    }
}
