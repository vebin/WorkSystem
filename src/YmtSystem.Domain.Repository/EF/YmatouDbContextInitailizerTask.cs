namespace YmtSystem.Domain.Repository.EF
{
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;
   
    /// <summary>
    /// 数据库初始化task
    /// </summary>
    public sealed class YmatouDbContextInitailizerTask : RegisterServiceBootstrapperTask
    {
        public YmatouDbContextInitailizerTask(IUnityContainer container) : base(container) { }
        public override int Order
        {
            get
            {
                //确保最先执行初始化
                return -1;
            }
        }

        public override TaskContinuation Execute()
        {
            YmatouDbContextInitailizer.Init();
            return TaskContinuation.Continue;
        }
    }
}
