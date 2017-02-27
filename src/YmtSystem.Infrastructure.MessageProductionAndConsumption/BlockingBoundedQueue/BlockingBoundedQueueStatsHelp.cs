/**********************************************************
 * Description:阻塞有界、生产消费协作队列,线程辅助跟踪
 * Author:lg
 * T:2012.8.28
 **********************************************************/


namespace YmtSystem.Infrastructure.MPAC.BlockingBoundedQueue
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.IO;
    using System.Text;

    internal class BlockingBoundedQueueStatsHelp
    {
        public static readonly object objLock = new object();

        public static void QueueStatsLogs(QueueStats q)
        {

            if (q == null) return;

#if DEBUGs
             var qMsg = string.Format(@"队列名称:{0},队列项数:{1},消费线程:{2},当前消费:{3},成功：{4},失败：{5},下次消费时间:{6},本次消费耗费时间:{7},出错时间:{8},消费发生错误的次数:{9},错误消息:{10}"
                   , q.QueueName, q.CurrentQueueCount, q.ConsumerThreadName, q.CurrentBeenConsumedCount,q.FailConsumedCount,q.SuccessConsumedCount
                   , q.NextConsumerTime, q.ConsumerUseTime, q.ErrorTime, q.ConsumerErrorItemCount, q.ErrorMessage);                      
            Console.WriteLine(qMsg);   

#else
            try
            {
                ThreadPool.QueueUserWorkItem(e =>
                {
                    QueueStats queueStats = e as QueueStats;
                    if (queueStats != null)
                    {
                        var qMsg = string.Format(@"队列名称:{0},队列项数:{1},消费线程:{2},当前消费:{3},成功：{4},失败：{5},下次消费时间:{6},本次消费耗费时间:{7}毫秒,服务端出错时间:{8},服务端错误的次数:{9},,服务端错误消息:{10},客户端Error:{11},客户端错误时间：{12}"
                    , q.QueueName, q.CurrentQueueCount, q.ConsumerThreadName, q.CurrentBeenConsumedCount, q.SuccessConsumedCount,q.FailConsumedCount 
                    , q.NextConsumerTime, q.ConsumerUseTime, q.ErrorTime, q.ConsumerErrorItemCount, q.ErrorMessage,q.ClientErrorMessage,q.ClientErrorTime);  
                        try
                        {
                          
                            Write(qMsg);
                        }
                        catch(Exception ex)
                        {
#if DEBUGs
                            Console.WriteLine(ex.Message);
#endif
                        }
                    }
                }, q);

            }
            catch
            {     
            }
#endif
        }

        private static void Write(String msg)
        {
            lock (objLock)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var currentTime = DateTime.Now.Minute;
                var i = 0;
                if (currentTime >= 0 && currentTime < 30)
                {
                    i = 0;
                }
                else if (currentTime >= 30 && currentTime <= 59)
                {
                    i = 1;
                }
                //var i = DateTime.Now.Minute % 30 == 0 ? 0 : 1;
                var logName = string.Format("{0}\\qt{1}_{2}.log", path, DateTime.Now.ToString("yyyyMMddHH"), i);
                using (StreamWriter s = new StreamWriter(logName, true, Encoding.GetEncoding("utf-8"), 1024))
                {
                    s.WriteLine(msg);
                    s.Flush();
                }
            }
        }
    }
}
