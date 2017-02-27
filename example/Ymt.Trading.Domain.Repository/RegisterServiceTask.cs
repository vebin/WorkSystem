namespace Ymt.Trading.Domain.Repository
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Practices.Unity;
    //Ymt.System
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.Repository;
    using YmtSystem.Repository.EF;
    //具体业务
    using Ymt.Trading.Domain.Model.Order;
    using Ymt.Trading.Domain.Repository;
    using Ymt.Trading.Domain.Repository.Mapping;
    using Ymt.Trading.Domain.Model.User;
    using Ymt.Trading.Domain.Repository.TradingContext;

    public class RegisterServiceTask : RegisterServiceBootstrapperTask
    {
        public RegisterServiceTask(IUnityContainer container) : base(container) { }
        public override TaskContinuation Execute()
        {
            container.RegisterType<IEFUnitOfWork, YmtTradingUnitOfwork>("order", new InjectionConstructor(""));

            container.RegisterType<IBankRepository, BankRepository>(new InjectionConstructor(new EFUnitOfWork("default")));
            // container.RegisterType<IYmtOrderRepository, YmtOrderRepository>( new InjectionConstructor(container.Resolve<IEFUnitOfWork>("order")));
            container.RegisterType<IYmtUserRepository, YmtUserRepository>(new InjectionConstructor(new EFUnitOfWork("default")));
            container.RegisterType<IBuyersRepository, BuyersRepository>(new InjectionConstructor(new EFUnitOfWork("default")));
            container.RegisterType<ITRepository, T1Repository>(new InjectionConstructor(new EFUnitOfWork("default")));
            container.RegisterType<IT2Repository, T2Repository>(new InjectionConstructor(new EFUnitOfWork("default")));
            container.RegisterType<IT5Repository, T5Repository>(new InjectionConstructor(new EFUnitOfWork("default")));
            container.RegisterType<IT6Repository, T6Repository>(new InjectionConstructor(new EFUnitOfWork("default")));
            container.RegisterType<IT8Repository, T8Repository>(new InjectionConstructor(new EFUnitOfWork("default")));
            container.RegisterType<IT11Repository, T11Repository>(new InjectionConstructor(new EFUnitOfWork("default")));
            container.RegisterType<IT14Repository, T14Repository>(new InjectionConstructor(new EFUnitOfWork("default")));

            //------------------------------
            //TODO:这里完成依赖注入
            //container.RegisterInstance<IBankRepository>(new BankRepository(new EFUnitOfWork()));
            //container.RegisterInstance<IYmtOrderRepository>(new YmtOrderRepository(new EFUnitOfWork()));
            //container.RegisterInstance<IYmtUserRepository>(new YmtUserRepository(new EFUnitOfWork()));
            //container.RegisterInstance<IBuyersRepository>(new BuyersRepository(new EFUnitOfWork()));
            //container.RegisterInstance<ITRepository>(new T1Repository(new EFUnitOfWork()));
            //container.RegisterInstance<IT2Repository>(new T2Repository(new EFUnitOfWork()));
            //container.RegisterInstance<IT5Repository>(new T5Repository(new EFUnitOfWork()));
            //container.RegisterInstance<IT6Repository>(new T6Repository(new EFUnitOfWork()));
            //container.RegisterInstance<IT8Repository>(new T8Repository(new EFUnitOfWork()));
            //container.RegisterInstance<IT11Repository>(new T11Repository(new EFUnitOfWork()));
            return TaskContinuation.Continue;
        }
    }
}
