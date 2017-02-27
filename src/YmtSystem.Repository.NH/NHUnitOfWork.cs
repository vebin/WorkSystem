namespace YmtSystem.Repository.NH
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NHibernate;
    using YmtSystem.CrossCutting;
    using YmtSystem.Domain.INHRepository;
    using YmtSystem.Repository.NH.Context;

    public abstract class NHUnitOfWork : AbstractUnitOfWork, INHUnitOfWork
    {
        private DbSessionContext _sessionFactory;

        public DbSessionContext CurrentSession { get { return _sessionFactory; } private set { _sessionFactory = value; } }

        public NHUnitOfWork(DbSessionContext sessionfactory)
        {
            this.CurrentSession = sessionfactory;
        }

        public override ResponseMessage<int> Commit(IsolationLevel? level = null, int retry = 1, bool lockd = false)
        {
            var transaction = this._sessionFactory.Current.BeginTransaction(level ?? this._sessionFactory._IsolationLevel);
            try
            {
                this.NewEntityList.Each(e => this._sessionFactory.Current.Save(e));
                this.ModifyEntityList.Each(e => this._sessionFactory.Current.Update(e));
                this.DeleteEntityList.Each(e => this._sessionFactory.Current.Delete(e));
                transaction.Commit();
                this.ClearAllEntity();
                return ResponseMessage<int>.CreateSuccess(1, message: "ok");
            }
            catch (Exception ex)
            {
                if (transaction.IsActive)
                    transaction.Rollback();
                this._sessionFactory.Current.Clear();
                return ResponseMessage<int>.CreateFail(-1, lastErrorMessage: ex.ToString());
            }
        }

        public override async Task<ResponseMessage<int>> AsyncCommit(int retry = 1)
        {
            return await Task.Factory.StartNew(() => Commit(null, retry, false));
        }

        public override void RollbackChanges()
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this._sessionFactory.Clear();
                this._sessionFactory.Close();
            }
        }

        public virtual void RegisterDeleted<TEntity>(params TEntity[] obj) where TEntity : class
        {
            obj.Each(e => base.RegisterDelete(e));
        }

        public virtual void RegisterModified<TEntity>(params TEntity[] obj) where TEntity : class
        {
            obj.Each(e => base.RegisterModified(e));
        }

        public virtual void RegisterNew<TEntity>(params TEntity[] obj) where TEntity : class
        {
            obj.Each(e => base.RegisterNew(e));
        }
    }
}
