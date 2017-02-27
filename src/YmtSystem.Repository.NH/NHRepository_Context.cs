namespace YmtSystem.Repository.NH
{
    using System;
    using System.Collections.Generic;
    using NHibernate;
    using YmtSystem.Domain.INHRepository;

    public partial class NHRepository<TEntity> where TEntity : class
    {
        private INHUnitOfWork _unitofWork;

        public NHRepository(IUnitOfWork unitofWork)
        {
            this._unitofWork = unitofWork as INHUnitOfWork;
        }
        public IUnitOfWork UnitOfWork { get { return this._unitofWork; } }

        private INHUnitOfWork _UnitOfWork { get { return this._unitofWork; } }

        protected ISession SessionContext { get { return this._unitofWork.CurrentSession.Current; } }
    }
}
