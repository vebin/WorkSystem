namespace YmtSystem.Infrastructure.EventStore
{
    using Microsoft.Practices.Unity;
    using YmtSystem.CrossCutting;
    using YmtSystem.Infrastructure.EventStore.Repository.Register;

    public class InitServiceTask : RegisterServiceBootstrapperTask
    {
        public InitServiceTask(IUnityContainer container) : base(container) { }

        public override TaskContinuation Execute()
        {
            RegisterMetaDataTask.ExecuteTask();
            return TaskContinuation.Continue;
        }
        protected override void InternalDispose()
        {
            DbContext.Clear();
            base.InternalDispose();
        }
    }
}
