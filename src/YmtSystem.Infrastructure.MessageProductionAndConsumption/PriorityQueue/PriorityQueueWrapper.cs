
/**********************************************************
 * Description:优先队列（二项队列） 线程管理
 * Author:lg
 * T:2012.8.28
 **********************************************************/

namespace YmtSystem.Infrastructure.MPAC.PQ
{
    using System;
    using System.Collections.Generic;
    using System.Linq;   
    using YmtSystem.Infrastructure.MPAC.PQ.Settings;
    using BlockingBoundedQueue;
    using System.Threading;
    using System.Diagnostics;
    using YmtSystem.CrossCutting;

    /// <summary>
    /// 优先队列
    /// </summary>                   
    public class PriorityQueueWrapper<K, V> where K : IComparable<K>
    {
        private bool init = false;
        private object lockObj = new object();
        private PQSettings pqSetting;
       //private int maxCount = 0;
        private List<Thread> thList;//= new List<Thread>();
        //private ConcurrentPriorityQueue<K, PQValue<K,V>> PQ;//= new ConcurrentPriorityQueue<K, V>();
        private ConcurrentPriorityQueueThreadManager<K, PQValue<K, V>> PQ;

        public PriorityQueueWrapper<K, V> Init(Action<List<Object>> action, string qName, PQSettings setting, bool isStart = true)
        {
            if (string.IsNullOrEmpty(qName)) throw new ArgumentNullException("队列名称不能空");
            if (!init)
            {
                lock (lockObj)
                {
                    if (!init)
                    {

                        YmatouLoggingService.Debug("开始初始化队列->{0}", qName);
                        if (setting == null)
                        {
                            pqSetting = new PQSettings
                            {
                                CFS = ConsumerFailStrategy.ClientHandleFail,
                                PQS = PQItemOverflowMaxStrategy.DeleteOldItem,
                                QName = qName,
                                ScanningTime = TimeSpan.FromMilliseconds(20),
                                ThreadCount = Environment.ProcessorCount,
                                ConsumerItemCount =3,
                                ConsumeAction = action,
                                MaxItems =100000
                            };
                        }
                        else
                        {
                            if (setting.ThreadCount > Environment.ProcessorCount || setting.ThreadCount<=0)
                                setting.ThreadCount = Environment.ProcessorCount;
                            if (setting.ScanningTime==new TimeSpan(0,0,0,0))
                                setting.ScanningTime = TimeSpan.FromSeconds(1);
                            if (setting.MaxItems > 100000 || setting.MaxItems<=0)
                                setting.MaxItems = 100000;
                            if (setting.ConsumerItemCount > setting.MaxItems || setting.ConsumerItemCount <= 0)
                                setting.ConsumerItemCount = 3;
                            setting.ConsumeAction = action;
                            setting.QName = qName;
                            pqSetting = setting;
                        }

                        thList = new List<Thread>(pqSetting.ThreadCount);
                        PQ = new ConcurrentPriorityQueueThreadManager<K, PQValue<K, V>>(pqSetting.MaxItems);
                        if (isStart)
                        {
                            Start();
                            PQStatus = Status.RunIng;
                        }
                        else
                        {
                            PQStatus = Status.Stop;
                        }
                        init = true;
                        YmatouLoggingService.Debug("完成初始化队列->{0}", qName);
                    }
                }
            }
            return this;
        }

        public PriorityQueueWrapper<K, V> Enqueue(K k, V v)
        {

            if (!init || PQStatus != Status.RunIng)
            {
                YmatouLoggingService.Debug("优先队列，{0}", !init || PQStatus != Status.RunIng);
                return this;
            }
            //while (PQ.Count >= pqSetting.MaxItems)
            //{
            //    if (pqSetting.PQS == PQItemOverflowMaxStrategy.DeleteOldItem)
            //        PQ.Dequeue();
            //    if (pqSetting.PQS == PQItemOverflowMaxStrategy.ExpandMaxCount)
            //        break;
            //    if (pqSetting.PQS == PQItemOverflowMaxStrategy.WaitConsumerItem)
            //    {

            //    }
            //}
            var pqObj = new PQValue<K, V>
            {
                ConsumerCount = 0,
                ConsumerResult = QueueItemConsumerStats.NoConsumer,
                Key = k,
                Value = v
            };
            PQ.Enqueue(k, pqObj);
            return this;

        }

        public PriorityQueueWrapper<K, V> BatchEnqueue(List<KeyValuePair<K, V>> v)
        {
            if (!init || PQStatus != Status.RunIng)
                return this;
            foreach (var item in v)            
                Enqueue(item.Key, item.Value);
            return this;
        }

        private void Consumer()
        {
            while (PQStatus == Status.RunIng)
            {
                try
                {
                    var itemCount = PQ.Count;
                    if (itemCount > 0)
                    {
                        var list = new List<Object>();
                        lock (PQ)
                        {
                            //启用了消费阀值 && 当前队列项数大于等于消费阀值
                            if (pqSetting.ConsumerItemCount > 0 && itemCount >= pqSetting.ConsumerItemCount)
                            {
                                for (var i = 0; i < pqSetting.ConsumerItemCount; i++)
                                {
                                   
                                    var v = PQ.Dequeue().Value;
                                    if (v != null)
                                        list.Add(v);
                                   
                                }
                            }
                            else if (itemCount < pqSetting.ConsumerItemCount)
                            {
                                for (var i = 0; i < itemCount; i++)
                                {
                                    var v = PQ.Dequeue().Value;
                                    if (v != null)
                                        list.Add(v);
                                }
                            }
                        }
                        if (list.Count > 0)
                        {
                            var _watch = Stopwatch.StartNew();
                            YmatouLoggingService.Debug("开始消费,qNmame->{0},itemCount->{1}", pqSetting.QName, list.Count);
                            try
                            {  
                                //lock(list)
                                pqSetting.ConsumeAction(list);
                            }
                            catch (QueueException e)
                            {
                                YmatouLoggingService.Debug(string.Format("PQService客户端消费错误,pqName->{0},msg->{1},stack->{2}", pqSetting.QName, e.Message, e.StackTrace));
                            }
                            YmatouLoggingService.Debug("结束消费,qNmame->{0},itemCount->{1},runTime->{2} ms", pqSetting.QName, list.Count, _watch.ElapsedMilliseconds);
                            _watch.Reset();
                            var _listQv = list.ConvertAll<PQValue<K, V>>(e => { return e as PQValue<K, V>; });
                            if (_listQv != null && _listQv.Count > 0)
                            {

                                var consumerNotCount = _listQv.Where(e => e != null && e.ConsumerResult == QueueItemConsumerStats.NoConsumer);
                                var consumerFailCount = _listQv.Where(e => e != null && e.ConsumerResult == QueueItemConsumerStats.Fail);
                                var tmpConsumerNotCount = consumerNotCount.Count();
                                var tmpconsumerFailCount = consumerFailCount.Count();
                                var consumerOkCount = _listQv.Count - tmpConsumerNotCount - tmpconsumerFailCount;
#if DEBUG
                                YmatouLoggingService.Debug("消费结果:总共->{0},未消费->{1},成功->{2},失败->{3}", list.Count, tmpConsumerNotCount, consumerOkCount, tmpconsumerFailCount);
#else 
                                QueueLogs.DebugFormat("消费结果:总共->{0},未消费->{1},成功->{2},失败->{3}", list.Count, tmpConsumerNotCount, consumerOkCount, tmpconsumerFailCount);
#endif


                                if (tmpConsumerNotCount > 0)
                                {
                                    foreach (var o in consumerNotCount)
                                    {
                                        PQ.Enqueue(o.Key, o);
                                    }
                                }

                                if (tmpconsumerFailCount > 0)
                                {
                                    if (pqSetting.CFS == ConsumerFailStrategy.Delet)
                                    {
                                    }
                                    else if (pqSetting.CFS == ConsumerFailStrategy.PersistenceEnqueue)
                                    {
                                       
                                    }
                                    else if (pqSetting.CFS == ConsumerFailStrategy.ClientHandleFail)
                                    {
                                    }
                                    else if (pqSetting.CFS == ConsumerFailStrategy.AgainEnqueue)
                                    {

                                        foreach (var o in consumerFailCount)
                                        {
                                            //消费失败次数累计                                            
                                            var tmpCount = o.ConsumerCount;
                                            Interlocked.Add(ref tmpCount, 1);
                                            o.ConsumerCount = tmpCount;
                                            if (o.ConsumerCount <= 3)
                                                PQ.Enqueue(o.Key, o);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (QueueException ex)
                {
                    YmatouLoggingService.Error(string.Format("PQService 服务端错误,pqName->{0},msg->{1},stack->{2}", pqSetting.QName, ex.Message, ex.StackTrace));
                }
                finally
                {
                    Thread.Sleep(pqSetting.ScanningTime);
                }
            }
        }

        public void Start()
        {
           
            if (init || PQStatus == Status.RunIng)
            {
                YmatouLoggingService.Warning("PQ 队列已启动");
                return;
            }
            if (thList.Count <= 0)
            {
                lock (thList)
                {
                    if (thList.Count <= 0)
                    {
                        YmatouLoggingService.Debug("启动队列开始 {0}", pqSetting.QName);
                        //thList = new List<Thread>();
                        for (var i = 0; i < pqSetting.ThreadCount; i++)
                        {
                            var th = new Thread(Consumer) { IsBackground = true, Name = "PQ_" + i };
                            th.Start();                                                   
                            thList.Add(th);
                        }
                        init = true;
                        PQStatus = Status.RunIng; 
                        YmatouLoggingService.Debug("启动队列结束 {0}", pqSetting.QName);
                    }
                }
            }
        }

        public void Stop()
        {
            lock (this)
            {
                try
                {
                    if (!init || PQStatus== Status.NoInit)
                    {                       
                        return;
                    }                   
                    //清除队列数据
                    if (PQ != null)
                        PQ.Clear();
                    //暴力终止
                    if (PQ.Count > 0)
                    {
                        foreach (var thObj in thList)
                        {
                               thObj.Join(10);
                        }
                        foreach (var thObj in thList)
                        {
                            try
                            {
                                thObj.Abort();
                            }
                            catch (ThreadAbortException e)
                            {

                            }
                            catch (ThreadInterruptedException ex)
                            {

                            }
                        }
                        thList.Clear();
                    }
                    PQStatus = Status.Stop;
                    init = false;                   
                }
                catch (QueueException e)
                {
                   YmatouLoggingService.Error("终止服务错误：{0},{1},{2}", "BBQW", e.Message, e.StackTrace);
                }
            }
        }
        public Status PQStatus { get; private set; }
    }
}
