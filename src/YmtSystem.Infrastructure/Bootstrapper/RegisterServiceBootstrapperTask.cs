﻿
namespace Ymatou.Infrastructure
{
    using Microsoft.Practices.Unity;

    public abstract class RegisterServiceBootstrapperTask : BootstrapperTask
    {
        public RegisterServiceBootstrapperTask(IUnityContainer container) : base(container) { }

        public override int Order
        {
            get
            {
                return 1;
            }
        }
    }
}
