namespace YmtSystem.Domain.INHRepository
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;

    /// <summary>
    /// unit Of Work 通用基类
    /// </summary>
    public abstract class AbstractUnitOfWork : DisposableObject
    {
        private readonly List<object> newEntity = new List<object>();
        private readonly List<object> modifyEntity = new List<object>();
        private readonly List<object> deletedEntity = new List<object>();

        protected virtual void ClearAllEntity()
        {
            this.newEntity.Clear();
            this.modifyEntity.Clear();
            this.deletedEntity.Clear();
        }

        protected virtual void RegisterNew(object entity)
        {
            if (!newEntity.Contains(entity))
            {
                newEntity.Add(entity);
            }
        }

        protected virtual void RegisterNew(object entity, IEqualityComparer<object> comparer)
        {
            if (!newEntity.Contains(entity, comparer))
            {
                newEntity.Add(entity);
            }
        }

        protected virtual void RegisterModified(object entity, IEqualityComparer<object> comparer)
        {
            if (!modifyEntity.Contains(entity, comparer))
            {
                modifyEntity.Add(entity);
            }
        }

        protected virtual void RegisterModified(object entity)
        {
            if (!modifyEntity.Contains(entity))
            {
                modifyEntity.Add(entity);
            }
        }

        protected virtual void RegisterDelete(object entity, IEqualityComparer<object> comparer)
        {
            if (!deletedEntity.Contains(entity, comparer))
            {
                deletedEntity.Add(entity);
            }
        }

        protected virtual void RegisterDelete(object entity)
        {
            if (!deletedEntity.Contains(entity))
            {
                deletedEntity.Add(entity);
            }
        }

        public abstract ResponseMessage<int> Commit(IsolationLevel? level = null, int retry = 1, bool lockd = false);
        public abstract Task<ResponseMessage<int>> AsyncCommit(int retry = 1);
        public abstract void RollbackChanges();

        protected List<object> NewEntityList { get { return newEntity; } }
        protected List<object> ModifyEntityList { get { return modifyEntity; } }
        protected List<object> DeleteEntityList { get { return deletedEntity; } }

        protected virtual ResponseMessage<int> InternalCommit(int retry = 1, bool lockd = false)
        {
            return Commit(null, retry, lockd);
        }

        protected virtual async Task<ResponseMessage<int>> InternalAsyncCommit(int retry = 1)
        {
            return await AsyncCommit(retry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) this.ClearAllEntity();
        }
    }
}
