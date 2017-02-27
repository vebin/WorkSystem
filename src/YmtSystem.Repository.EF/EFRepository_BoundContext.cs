namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.Repository;
    using YmtSystem.Domain.Shard;

    public partial class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public EFRepository(IUnitOfWork unitofwork)
        {
            if (unitofwork is IUnitOfWork)
                this.unitofwork = unitofwork as IEFUnitOfWork;
            else
                throw new Exception<EFRepositoryException>("仓储类型错误");
        }
        [Obsolete("use UnitOfWork")]
        public IUnitOfWork Context
        {
            get { return unitofwork; }
        }
        public IUnitOfWork UnitOfWork
        {
            get { return unitofwork; }
        }
        public void Dispose()
        {
            if (unitofwork != null)
                unitofwork.Dispose();
            YmtSystem.CrossCutting.YmatouLoggingService.Debug("EFRepository type {0} Dispose", typeof(TEntity).FullName);
        }
       
        private IEFUnitOfWork unitofwork;
        protected IEFUnitOfWork EfUnitOfWork { get { return this.unitofwork; } }
    }
}
