using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace YmtSystem.Infrastructure.Aop.Monitor
{
    public class MethodRunTimeMonitorHandlerAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new MethodRunTimeMonitorHandler { Order = this.Order, Compare = this.Compare, Message = this.Message };
        }
        /// <summary>
        /// 大于这个值则记录日志
        /// </summary>
        public int Compare { get; set; }
        public string Message { get; set; }
    }
}
