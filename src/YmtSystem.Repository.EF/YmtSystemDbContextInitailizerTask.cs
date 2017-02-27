namespace YmtSystem.Repository.EF
{
    using System;
    using YmtSystem.CrossCutting;
    using Microsoft.Practices.Unity;
    using System.Web;
    using System.Data.Entity.Infrastructure.Interception;
    using YmtSystem.Repository.EF.Factory;
    /// <summary>
    /// 数据库初始化task
    /// </summary>
    public sealed class YmtSystemDbContextInitailizerTask : RegisterServiceBootstrapperTask
    {
        public YmtSystemDbContextInitailizerTask(IUnityContainer container) : base(container) { }
        public override int Order
        {
            get
            {
                //确保最先执行初始化
                return -99;
            }
        }

        public override TaskContinuation Execute()
        {
            //YmatouLoggingService.InitLogService();
            //this.ReisterDbContext();
            YmtSystemDbContextInitailizer.Init();
            //SqlCommandInterceptorService.Start("");
            //YmtSystemSqlExecuteFailStrategyService.Start();
            container.RegisterTypeAsSingleton<IDbContextFactory, DbContextFactory>();
            container.RegisterTypeAsSingleton<IRepositoryFactory, RepositoryFactory>();
            return TaskContinuation.Continue;
        }
        protected override void InternalDispose()
        {
            YmtSystemDbContextScope<YmtSystemDbContext>.TryClear();
            base.InternalDispose();
        }
        private void ReisterDbContext()
        {
            #region [ PerResolveLifetimeManager ]
            //PerResolveLifetimeManager 解决循环引用而重复引用的生命周期,
            //似于TransientLifetimeManager，但是其不同在于，如果应用了这种生命周期管理器，则在第一调用的时候会创建一个新的对象，
            //而再次通过循环引用访问到的时候就会返回先前创建的对象实例（单件实例）
            /**
             **
             *  public interface IPresenter { }
                public class MockPresenter : IPresenter
                {
                    public IView View { get; set; }

                    public MockPresenter(IView view)
                    {
                        View = view;
                    }
                }

                public interface IView
                {
                    IPresenter Presenter { get; set; }
                }

                public class View : IView
                {
                    [Dependency]
                    public IPresenter Presenter { get; set; }
                }
             * 
             * container.RegisterType<YmtSystemDbContext, YmtSystemDbContext>(new PerResolveLifetimeManager());
             * return
             ****/
            //PerThreadLifetimeManager 对每个线程都是唯一的
            #endregion

            if (HttpContext.Current != null)
            {
                YmatouLoggingService.Debug("DbContext HttpContextLifetimeManager");
                container.RegisterType<YmtSystemDbContext, YmtSystemDbContext>(new HttpContextLifetimeManager<YmtSystemDbContext>());
            }
            else
            {
                YmatouLoggingService.Debug("DbContext PerThreadLifetimeManager");
                container.RegisterType<YmtSystemDbContext, YmtSystemDbContext>(new PerThreadLifetimeManager());
            }
        }
    }
}
