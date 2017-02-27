namespace Ymatou.Infrastructure
{
    using Microsoft.Practices.Unity;
    public abstract class InitServiceBootstrapperTask : BootstrapperTask
    {
        public InitServiceBootstrapperTask(IUnityContainer container) : base(container) { }

        public override int Order
        {
            get
            {
                return 4;
            }
        }
    }
}
