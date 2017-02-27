/**********************************************************
 * Description:阻塞有界、生产消费协作队列,参数设置
 * Author:lg
 * T:2012.8.28
 **********************************************************/

namespace YmtSystem.Infrastructure.MPAC.BlockingBoundedQueue
{
    using System;
    using System.Collections.Generic;  
    using System.Threading;

    /// <summary>
    /// 队列配置；
    /// 对于策略只配置消费失败策略就行了；消费数据不足，生产超过上限这两种情况程序内部默认处理为：
    /// 消费不足，等待一定时间让生产者生产数据；生产过量，等待一定时间让消费者消费，如果没有消费者就删掉老数据（等待时间大于消费时间）
    /// </summary>
    public class BlockingBoundedQueueSettings
    {     
        /// <summary>
        /// 消费线程数
        /// </summary>
        public int ConsumerThreadCount { get; set; }     
        /// <summary>
        /// 消费失败策略
        /// </summary>
        public ConsumerFailStrategy ConsumerFS { get; set; }
        /// <summary>
        /// 一次消费数量
        /// </summary>
        public int ConsumerItemCount { get; set; }
        /// <summary>
        /// 消费间隔时间
        /// </summary>
        public TimeSpan ConsumerTime { get; set; }
        /// <summary>
        /// 消费回调方法
        /// </summary>
        public Action<List<object>> ConsumerAction { get; set; }       
        /// <summary>
        /// 队列最大项数
        /// </summary>
        public int QueueItemMaxCount { get; set; }
        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }
     
    }

    public class QueueStats
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// 当前队列项数
        /// </summary>
        public long CurrentQueueCount { get; set; }
        /// <summary>
        /// 消费线程名
        /// </summary>
        public string ConsumerThreadName { get; set; }        
        /// <summary>
        /// 当前被消费的项目数
        /// </summary>
        public long CurrentBeenConsumedCount { get; set; }
        /// <summary>
        /// 成功消费数量
        /// </summary>
        public long SuccessConsumedCount { get; set; }
        /// <summary>
        /// 消费失败数量
        /// </summary>
        public long FailConsumedCount { get; set; }
        /// <summary>
        /// 下次消费时间
        /// </summary>
        public DateTime NextConsumerTime { get; set; }
        /// <summary>
        /// 本次消费耗费时间
        /// </summary>
        public long ConsumerUseTime { get; set; }
        /// <summary>
        /// 出错时间        
        /// </summary>
        public DateTime ErrorTime { get; set; }
        /// <summary>
        /// 消费发生错误的次数
        /// </summary>
        public long ConsumerErrorItemCount { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 客户端消费数据错误时间
        /// </summary>
        public DateTime ClientErrorTime { get; set; }
        /// <summary>
        /// 客户端消费数据错误消息
        /// </summary>
        public string ClientErrorMessage { get; set; }
    }
    
    public class QueueValue<T>
    {
        public T Value { get; set; }
        public QueueItemConsumerStats ConsumerResult { get; set; }
        internal int ConsumerCount { get; set; }
    }
}
