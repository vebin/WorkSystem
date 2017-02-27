namespace YmtSystem.Infrastructure.EventBusServiceV2
{
    using System;

    public class EventHandlerStrategy
    {
        public EventHandlerStrategy()
        {
            AsyncReceive = false;
            PersistentStore = false;
            Retry = 1;
        }

        /// <summary>
        /// 异步接收事件
        /// </summary>
        public bool AsyncReceive { get; set; }
        /// <summary>
        /// 重复执行次数
        /// </summary>
        public int Retry { get; set; }
        /// <summary>
        /// 订阅类型事件
        /// </summary>
        public Type SubEventType { get; set; }
        /// <summary>
        /// 是否持久化存储事件
        /// </summary>
        public bool? PersistentStore { get; set; }
        /// <summary>
        /// 处理事件超时时间
        /// </summary>
        public TimeSpan? TimeOut { get; set; }
    }
}
