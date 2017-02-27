//namespace YmtSystem.Repository
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Collections.Concurrent;
//    using System.Linq;
//    using System.Text;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using YmtSystem.CrossCutting;
//    using YmtSystem.Domain.Shard;
//    using YmtSystem.Domain.Repository;
//    using System.Transactions;

//    /// <summary>
//    /// 工作单元CRUD伪实现基类。    
//    /// <remarks>在内存中暂存了聚合跟等 commit 的时候，才提交暂存的全部内容；</remarks>   
//    /// </summary>
//    public abstract class AbstractUnitOfWork : DisposableObject
//    {
//        private readonly ConcurrentDictionary<IAggregateRoot, IUnitOfWork> newEntity = new ConcurrentDictionary<IAggregateRoot, IUnitOfWork>();
//        private readonly ConcurrentDictionary<IAggregateRoot, IUnitOfWork> modifyEntity = new ConcurrentDictionary<IAggregateRoot, IUnitOfWork>();
//        private readonly ConcurrentDictionary<IAggregateRoot, IUnitOfWork> deletedEntity = new ConcurrentDictionary<IAggregateRoot, IUnitOfWork>();

//        public AbstractUnitOfWork()
//        {

//        }

//        /// <summary>
//        /// 清除仓储中所有注册对象.
//        /// </summary>      
//        protected void ClearRegistrations()
//        {
//            this.newEntity.Clear();
//            this.modifyEntity.Clear();
//            this.deletedEntity.Clear();
//        }
//        /// <summary>
//        /// Disposes the object.
//        /// </summary>
//        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
//        /// the object should be disposed explicitly.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                ClearRegistrations();
//            }
//        }

//        /// <summary>
//        /// 同步提交；这里交给真正实现工作单元的类去提交
//        /// </summary>
//        /// <param name="retry"></param>
//        /// <returns></returns>
//        protected abstract ExecuteResult<int> DoCommit(int retry = 3);
//        /// <summary>
//        /// 异步提交；这里交给真正实现工作单元的类去提交
//        /// </summary>
//        /// <param name="retry"></param>
//        /// <returns></returns>
//        protected abstract Task<ExecuteResult<int>> DoAsyncCommit(int retry = 3);

//        /// <summary>
//        /// 回滚所有状态
//        /// </summary>
//        public abstract void RollbackChanges();

//        protected IEnumerable<IAggregateRoot> NewCollection
//        {
//            get { return newEntity.Keys.Select(e => e); }
//        }

//        protected IEnumerable<IAggregateRoot> ModifiedCollection
//        {
//            get { return modifyEntity.Keys.Select(e => e); }
//        }

//        protected IEnumerable<IAggregateRoot> DeletedCollection
//        {
//            get { return deletedEntity.Keys.Select(e => e); }
//        }

//        /// <summary>
//        /// 状态注册为”新建“.
//        /// </summary>      
//        public virtual void RegisterNew<TAggregateRoot>(TAggregateRoot obj, IUnitOfWork unitOfWork) where TAggregateRoot : class, IAggregateRoot
//        {
//            if (obj == null) throw new NullReferenceException("聚合根不能为空");
//            if (modifyEntity.ContainsKey(obj))
//                return;
//            if (!newEntity.ContainsKey(obj))
//                newEntity.TryAdd(obj, unitOfWork);
//        }
//        /// <summary>
//        /// 注册为“修改状态”
//        /// </summary>     
//        public virtual void RegisterModified<TAggregateRoot>(TAggregateRoot obj, IUnitOfWork unitOfWork) where TAggregateRoot : class, IAggregateRoot
//        {
//            if (obj == null) throw new NullReferenceException("聚合根不能为空");
//            if (deletedEntity.ContainsKey(obj))
//                return;
//            modifyEntity.AddOrUpdate(obj, unitOfWork, (key, sunobj) => sunobj = unitOfWork);
//        }

//        /// <summary>
//        /// 注册为“删除状态”
//        /// </summary>      
//        public virtual void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj, IUnitOfWork unitOfWork) where TAggregateRoot : class, IAggregateRoot
//        {
//            IUnitOfWork val;
//            if (obj == null)
//                throw new ArgumentException("聚合根ID不能为空", "obj");
//            if (newEntity.ContainsKey(obj))
//                newEntity.TryRemove(obj, out val);
//            if (modifyEntity.ContainsKey(obj))
//                modifyEntity.TryRemove(obj, out val);
//            if (!deletedEntity.ContainsKey(obj))
//                deletedEntity.TryAdd(obj, unitOfWork);
//        }


//        public virtual ExecuteResult<int> Commit(int retry = 3)
//        {
//            using (var scope = new TransactionScope())
//            {
//                newEntity.Each(
//                    e =>
//                    e.Value.Commit()
//                    );
//                modifyEntity.Each(
//                   e =>
//                   e.Value.Commit()
//                   );
//                deletedEntity.Each(
//                   e =>
//                   e.Value.Commit()
//                   );

//                scope.Complete();
//            }
//            return this.DoCommit(retry);
//        }

//        public virtual Task<ExecuteResult<int>> AsyncCommit(int retry = 3)
//        {
//            return this.DoAsyncCommit(retry);
//        }
//    }
//}
