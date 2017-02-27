/**********************************************************
 * Description:队列服务枚举关键字
 * Author:lg
 * T:2012.8.28
 **********************************************************/

namespace YmtSystem.Infrastructure.MPAC.BlockingBoundedQueue
{
    using System;

    public enum QueueItemConsumerStats
    {
        /// <summary>
        /// 未消费
        /// </summary>
        NoConsumer = 0,
        /// <summary>
        /// 消费成功
        /// </summary>
        Ok = 1,
        /// <summary>
        /// 消费失败
        /// </summary>
        Fail = 2
    }

    /// <summary>
    /// 消费失败策略
    /// </summary>
    public enum ConsumerFailStrategy
    {
        /// <summary>
        /// 客户端处理消费的数据
        /// </summary>
        ClientHandleFail = 0,
        /// <summary>
        /// 删掉错误数据
        /// </summary>
        Delet=1,
        /// <summary>
        /// 重新入列3次
        /// </summary>
        AgainEnqueue = 2,
        /// <summary>
        /// 持久入列（没有意义）
        /// </summary>
        PersistenceEnqueue = 3

    }

    /// <summary>
    /// 数据超过最大项数策略
    /// </summary>
    public enum PQItemOverflowMaxStrategy
    {
        /// <summary>
        /// 删除老数据
        /// </summary>
        DeleteOldItem = 0,
        /// <summary>
        /// 等待消费
        /// </summary>
        WaitConsumerItem = 1,
        /// <summary>
        /// 扩大一次队列上限
        /// </summary>
        ExpandMaxCount = 2
    }

    public enum Status
    {
        NoInit=0,
        RunIng=1,
        Stop=2
    }
}
