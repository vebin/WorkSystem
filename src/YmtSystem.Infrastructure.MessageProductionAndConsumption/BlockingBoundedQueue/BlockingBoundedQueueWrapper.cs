/**********************************************************
 * Description:阻塞有界、生产消费协作队列,线程管理
 * Author:lg
 * T:2012.8.28
 **********************************************************/

namespace YmtSystem.Infrastructure.MPAC.BlockingBoundedQueue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Linq;
    using System.Diagnostics;
    using YmtSystem.CrossCutting;

    public sealed class BlockingBoundedQueueWrapper<T> 
    {
        //private static ILog LOG = LogManager.GetLogger(typeof(BlockingBoundedQueueWrapper<T>));
        private List<Thread> _thList;                      // = new List<Thread>();
        private BlockingBoundedQueue<QueueValue<T>> _queue;// = new BlockingBoundedQueue<QueueValue<T>>(100000);       
        private int _consumerFailCount = 3; 
        private BlockingBoundedQueueSettings _queueSettings;
        private object lockObj = new object();
        private object _lockInit = new object();       
        private long _errorCount = 0;
        private bool _enable = true;
        private bool _isInit = false;
        private bool _consumerStats = false;
        private const string version = "2012.10.22";
        public BlockingBoundedQueueWrapper()
        {
                      
        }


        public BlockingBoundedQueueWrapper<T> Init(Action<List<Object>> consumerAction, string queueName, BlockingBoundedQueueSettings queueSettings,bool isStart=true)
        {
            if (string.IsNullOrEmpty(queueName)) throw new ArgumentNullException("队列名称不能空");
            if (!_isInit)
            {
                lock (_lockInit)
                {
                    if (!_isInit)
                    {

                        YmatouLoggingService.Debug("开始初始化队列,{0}", queueName);
                        if (consumerAction == null)
                        {
                            throw new ArgumentException("必须提供消费回调方法");
                        }
                        if (queueSettings == null)
                        {
                            _queueSettings = new BlockingBoundedQueueSettings
                            {
                                ConsumerAction = consumerAction,
                                ConsumerThreadCount = Environment.ProcessorCount,
                                ConsumerTime = new TimeSpan(0, 0, 0, 0, 10),
                                QueueName = queueName,
                                ConsumerItemCount = 3,
                                QueueItemMaxCount = 100000,
                                ConsumerFS= ConsumerFailStrategy.AgainEnqueue
                            };
                        }
                        else
                        {
                            if (queueSettings.ConsumerThreadCount <= 0 || queueSettings.ConsumerThreadCount > Environment.ProcessorCount)
                                queueSettings.ConsumerThreadCount = Environment.ProcessorCount;
                            if (queueSettings.ConsumerTime < new TimeSpan(0, 0, 0, 10))
                                queueSettings.ConsumerTime = new TimeSpan(0, 0, 0, 0, 10);
                            if (queueSettings.ConsumerItemCount <= 0)
                                queueSettings.ConsumerItemCount = 1;
                            if (queueSettings.QueueItemMaxCount > 100000 || queueSettings.QueueItemMaxCount <= 0)
                                queueSettings.QueueItemMaxCount = 100000;
                            if (string.IsNullOrEmpty(queueSettings.QueueName))
                                queueSettings.QueueName = queueName;
                            if (queueSettings.ConsumerAction == null)
                                queueSettings.ConsumerAction = consumerAction;

                            _queueSettings = queueSettings;
                        }
                        _thList = new List<Thread>();
                        _queue = new BlockingBoundedQueue<QueueValue<T>>(_queueSettings.QueueItemMaxCount);
                        if (isStart)
                        {
                            this.Start();
                            BQStatus = Status.RunIng;
                        }
                        else
                        {
                            BQStatus = Status.Stop;
                        }
                        _isInit = true;
                        YmatouLoggingService.Debug("Queue Service Version {0}", version);
                        YmatouLoggingService.Debug("结束初始化队列,{0}", queueName);
                    }
                }
            }
            return this;
        }
    
        public BlockingBoundedQueueWrapper<T> Enqueue(T t)
        {
            if (!_isInit || BQStatus!= Status.RunIng) throw new QueueException("Not Init Queue");
            var qV = new QueueValue<T> { Value = t, ConsumerCount = 0, ConsumerResult = QueueItemConsumerStats.NoConsumer };
            _queue.Enqueue(qV);
            return this;
        }

        public BlockingBoundedQueueWrapper<T> BatchEnqueue(List<T> t)
        {
            if (!_isInit || BQStatus != Status.RunIng) throw new QueueException("Not Init Queue");
            foreach (var item in t)
                Enqueue(item);
            return this;
        }

        private void Consumer()
        {
            while (BQStatus== Status.RunIng)
            {

                _consumerStats = true;
                var _consumerTime = _queueSettings.ConsumerTime;
                var _consumerItemCount = _queueSettings.ConsumerItemCount;

                //记录一次消费的状态
                var _currentQueueStats = new QueueStats();
                _currentQueueStats.QueueName = _queueSettings.QueueName;
                _currentQueueStats.ConsumerThreadName = Thread.CurrentThread.Name;
              
                var _listConsumerItem = new List<Object>(_consumerItemCount);
                try
                {
                    var _queueCount = _queue.Count;
                   
                    _currentQueueStats.CurrentQueueCount = _queueCount;
                    lock (_queue)
                    {
                        //批量消费策略
                        if (_queueCount > 0)
                        {
                            if (_queueCount >= _consumerItemCount)
                            {
                                for (var i = 0; i < _consumerItemCount; i++)
                                {
                                    var v = _queue.Dequeue();
                                    if (v != null)
                                        _listConsumerItem.Add(v);
                                }
                            }
                            else
                            {
                                //TODO:生产不足策略:等待数据到来，或消费掉不足的所有数据 
#if DEBUG
                                YmatouLoggingService.Debug("生产数据不足:{0},{1}，{2}", _queueSettings.QueueName, _consumerItemCount, _queueCount);
#endif
                                for (var i = 0; i < _queueCount; i++)
                                {
                                    var v = _queue.Dequeue();
                                    if (v != null)
                                        _listConsumerItem.Add(v);
                                }
                            }
                        }
                    }
                    if (_listConsumerItem.Count > 0)
                    {
                        //单个消费
                        //var item = _queue.Dequeue();
                        try
                        {
                            //监控
                            var _watch = Stopwatch.StartNew();
                            //客户端消费数据
                            try
                            {
                                YmatouLoggingService.Debug("进行一次消费 {0}-> {1} 项", _queueSettings.QueueName, _listConsumerItem.Count);
                                _queueSettings.ConsumerAction(_listConsumerItem);
                            }
                            catch (QueueException ce)
                            {
                                _currentQueueStats.ClientErrorMessage = ce.Message;
                                _currentQueueStats.ClientErrorTime = DateTime.Now;
                                YmatouLoggingService.Debug("客户端消费错误：{0},{1},{2}", _queueSettings.QueueName, ce.Message, ce.StackTrace);
                            }
                            catch(Exception e)
                            {
                                _currentQueueStats.ClientErrorMessage = e.Message;
                                _currentQueueStats.ClientErrorTime = DateTime.Now;
                                YmatouLoggingService.Debug("客户端消费错误：{0},{1},{2}", _queueSettings.QueueName, e.Message, e.StackTrace);
                            }
                            _currentQueueStats.ConsumerUseTime = _watch.ElapsedMilliseconds;
                            _watch.Stop();

                            //一次消费完成，状态跟踪
                            //统计总共消费
                            //Interlocked.Add(ref _beenConsumedCount, _listConsumerItem.Count);
                            var _listQv = _listConsumerItem.ConvertAll<QueueValue<T>>(e => { return e as QueueValue<T>; });
                            if (_listQv.Count > 0)
                            {
                                _currentQueueStats.SuccessConsumedCount = _listQv.Count(e =>e!=null && e.ConsumerResult == QueueItemConsumerStats.Ok);

                                _currentQueueStats.FailConsumedCount = _listQv.Count(e => e != null && e.ConsumerResult != QueueItemConsumerStats.Ok);
                                _currentQueueStats.CurrentBeenConsumedCount = _listConsumerItem.Count;
                                //一次消费完成，对消费失败的数据再处理
                                if (_queueSettings.ConsumerFS != ConsumerFailStrategy.ClientHandleFail)
                                {
                                    if (_queueSettings.ConsumerFS == ConsumerFailStrategy.Delet)
                                    {
                                    }
                                    else if (_queueSettings.ConsumerFS == ConsumerFailStrategy.PersistenceEnqueue)
                                    {
                                    }
                                    else
                                    {
                                        var _listFailItem = _listQv.Where(e =>e!=null && e.ConsumerResult != QueueItemConsumerStats.Ok && e.ConsumerCount <= _consumerFailCount);
                                        foreach (var item in _listFailItem)
                                        {
                                            var _errorCount = item.ConsumerCount;
                                            Interlocked.Add(ref _errorCount, 1);
                                            item.ConsumerCount = _errorCount;
                                            if (item.ConsumerCount < _consumerFailCount)
                                                _queue.Enqueue(item);
                                        }
                                    }
                                }
                            }
                        }
                        catch (QueueException e)
                        {
#if !DEBUG
                            QueueLogs.ErrorFormat("队列服务错误,{0},{1},{2}",_queueSettings.QueueName, e.Message,e.StackTrace);
#endif
                            _currentQueueStats.ErrorTime = DateTime.Now;
                            _currentQueueStats.ErrorMessage = e.Message;
                            Interlocked.Add(ref _errorCount, 1);
                            _currentQueueStats.ConsumerErrorItemCount = _errorCount;
                            var _listQv = _listConsumerItem.ConvertAll<QueueValue<T>>(_ => { return _ as QueueValue<T>; });
                            foreach (var item in _listQv)
                            {                               
                                _queue.Enqueue(item);
                            }
                        }
                    }
                }
                catch (QueueException ex)
                {
#if !DEBUG
                     QueueLogs.ErrorFormat("客户端消费错误2：{0},{1},{2}", _queueSettings.QueueName,ex.Message, ex.StackTrace);
#endif
                    _currentQueueStats.ErrorMessage += "" + ex.Message;
                    _currentQueueStats.ErrorTime = DateTime.Now;
                }
                finally
                {
                    _currentQueueStats.NextConsumerTime = DateTime.Now.Add(_consumerTime);
#if DEBUGs
                    BlockingBoundedQueueStatsHelp.QueueStatsLogs(_currentQueueStats);
#endif
                                      
                    Thread.Sleep(_consumerTime);
                }
            }
        }

        public void Start()
        {
            if (_isInit && BQStatus == Status.RunIng) return;

            if (_thList.Count <= 0)
            {
                lock (lockObj)
                {
                    if (_thList.Count <= 0)
                    {
                        YmatouLoggingService.Debug("启动队列,{0}", _queueSettings.QueueName);
                        for (var i = 0; i < this._queueSettings.ConsumerThreadCount; i++)
                        {
                            var _th = new Thread(Consumer) { IsBackground = true, Name = string.Format("{0}{1}", _queueSettings.QueueName, i) };
                            //_th.Start();
                            _thList.Add(_th);
                        }
                        _isInit = true;
                        BQStatus = Status.RunIng;
                        _thList.ForEach(e => e.Start());
                        YmatouLoggingService.Debug("启动完成,{0}", _queueSettings.QueueName);
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
                    if (!_isInit)
                    {
                        YmatouLoggingService.Debug("服务未初始化无需清理");
                        return;
                    }
                    YmatouLoggingService.Debug("开始清理");
                    //清除队列数据
                    if (_queue != null)
                        _queue.Clear();
                    //暴力终止
                    if (_thList.Count > 0)
                    {
                        foreach (var thObj in _thList)
                        {
                            thObj.Join(10);
                        }
                        foreach (var thObj in _thList)
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
                        _thList.Clear();
                    }
                    _consumerStats = false;
                    _isInit = false;
                    BQStatus = Status.Stop;
                    YmatouLoggingService.Debug("清理完成");
                }
                catch (QueueException e)
                {
                    YmatouLoggingService.Debug("终止服务错误：{0},{1},{2}", "BBQW", e.Message, e.StackTrace);
                }
            }
        }

        public Status BQStatus { get; private set; }
    }
}
