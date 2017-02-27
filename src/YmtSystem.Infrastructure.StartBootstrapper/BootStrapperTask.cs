namespace YmtSystem.Infrastructure.Bootstrapper
{
    using YmtSystem.CrossCutting.Utility;
    using YmtSystem.CrossCutting;

    public abstract class BootstrapperTask : Disposable
    {
        public BootstrapperTask()
        {
        }
        /// <summary>
        /// 执行顺序
        /// </summary>
        public virtual int Order
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Description
        {
            get
            {
                return "";
            }
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public abstract TaskContinuation Execute();
    }
}
