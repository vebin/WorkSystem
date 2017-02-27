using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace YmtSystem.Infrastructure.Aop.Monitor
{
    public class MethodRunTimeMonitorHandler : ICallHandler
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var arguments = new object[input.Arguments.Count];
            input.Arguments.CopyTo(arguments, 0);
            var watch = Stopwatch.StartNew();
            var r = getNext()(input, getNext);
            YmtSystem.CrossCutting.YmatouLoggingService.Debug("方法->{0},参数->{1},耗时->{2}", input.MethodBase.Name, string.Join<object>(",", arguments), watch.Elapsed.TotalSeconds);
            //Console.WriteLine("方法->{0},参数->{1},耗时->{2}", input.MethodBase.Name, string.Join<object>(",", arguments), watch.Elapsed.TotalSeconds);
            return r;
        }

        public int Order { get; set; }
        /// <summary>
        /// 大于这个值则记录日志
        /// </summary>
        public int Compare { get; set; }
        public string Message { get; set; }
    }
}