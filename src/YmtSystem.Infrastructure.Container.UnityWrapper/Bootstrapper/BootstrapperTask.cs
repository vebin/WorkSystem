namespace YmtSystem.Infrastructure.Container.UnityWrapper
{
    using YmtSystem.CrossCutting.Utility;
    using Microsoft.Practices.Unity;

    public abstract class BootstrapperTask : Disposable
    {
        protected IUnityContainer container;

        public BootstrapperTask(IUnityContainer container)
        {
            this.container = container;
        }

        public virtual int Order
        {
            get
            {
                return 0;
            }
        }

        public virtual string Description
        {
            get
            {
                return "";
            }
        }

        public abstract TaskContinuation Execute();
    }
}
