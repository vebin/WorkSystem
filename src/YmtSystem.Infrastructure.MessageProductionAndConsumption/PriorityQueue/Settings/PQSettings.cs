/**********************************************************
* Description:优先队列（二项队列） 配置
* Author:lg
* T:2012.8.28
**********************************************************/

namespace YmtSystem.Infrastructure.MPAC.PQ.Settings
{
    using System;
    using BlockingBoundedQueue;
    using System.Collections.Generic;
    using System.Threading;

    public class PQSettings
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string QName { get; set; }
        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount { get; set; }
        /// <summary>
        /// 扫描队列时间
        /// </summary>
        public TimeSpan ScanningTime { get; set; }
        /// <summary>
        /// 消费阀值
        /// </summary>
        public int ConsumerItemCount { get; set; }
        /// <summary>
        ///数据超过最大项数策略 
        /// </summary>
        public PQItemOverflowMaxStrategy PQS { get; set; }
        /// <summary>
        /// 消费失败策略
        /// </summary>
        public ConsumerFailStrategy CFS { get; set; }
        /// <summary>
        /// 最大项
        /// </summary>
        public int MaxItems { get; set; }
        /// <summary>
        /// 消费方法
        /// </summary>
        public Action<List<Object>> ConsumeAction { get; set; }
        /// <summary>
        /// 线程游戏级别
        /// </summary>
        //public ThreadPriority TP { get; set; }
        /// <summary>
        /// 消费方法
        /// </summary>
        //public Action<Object> ConsumeAction { get; set; }
    } 
}
