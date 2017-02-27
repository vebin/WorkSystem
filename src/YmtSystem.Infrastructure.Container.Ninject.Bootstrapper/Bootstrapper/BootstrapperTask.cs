namespace YmtSystem.Infrastructure.Container._Ninject.Bootstrapper
{
    using Ninject;
    using Ninject.Modules;
    using YmtSystem.CrossCutting.Utility;

    public abstract class BootstrapperTask : NinjectModule
    {
        protected static IKernel kernel;

        public BootstrapperTask(IKernel container)
        {
            kernel = container;
        }
        public BootstrapperTask Modul { get { return this; } }

        public virtual int Order { get { return 0; } }
        public virtual string Description { get { return string.Empty; } }

        public abstract TaskContinuation Execute();

        public override void Load()
        {
            Execute();
        }
    }
}
