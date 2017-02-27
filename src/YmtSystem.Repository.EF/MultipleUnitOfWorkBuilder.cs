namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Repository;
    using YmtSystem.Domain.Shard;

    public sealed class MultipleUnitOfWorkBuilder : IDisposable
    {
        private readonly List<IUnitOfWork> pool = new List<IUnitOfWork>();
        private MultipleUnitOfWorkBuilder()
        {

        }
        [ThreadStatic]
        private static MultipleUnitOfWorkBuilder instance;

        public static MultipleUnitOfWorkBuilder Instance { get { if (instance == null)instance = new MultipleUnitOfWorkBuilder(); return (instance = new MultipleUnitOfWorkBuilder()); } }

        public MultipleUnitOfWorkBuilder Append(Func<IUnitOfWork> unitOfWork)
        {
            var tmpUnitOfWork = unitOfWork();
            if (tmpUnitOfWork != null)
                Append(tmpUnitOfWork);
            return this;
        }
        public MultipleUnitOfWorkBuilder Append(IUnitOfWork unitOfWork)
        {
            try
            {
                pool.Add(unitOfWork);
                return this;
            }
            catch (Exception ex)
            {
                YmtSystem.CrossCutting.YmatouLoggingService.Error("MultipleUnitOfWorkBuilder error {0}", ex.ToString());
                throw;
            }
        }
        public MultipleUnitOfWorkBuilder Append(IEnumerable<IUnitOfWork> unitOfWork)
        {
            try
            {
                unitOfWork.EachHandle(e => pool.Add(e));
                return this;
            }
            catch (Exception ex)
            {
                YmtSystem.CrossCutting.YmatouLoggingService.Error("MultipleUnitOfWorkBuilder error {0}", ex.ToString());
                throw;
            }
        }
        public IDisposable Commit()
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                pool.ForEach(e => e.Commit());
                scope.Complete();
            }
            return this;
        }

        public void Commit(params Action[] commit)
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                commit.EachHandle(e =>
                    e()
                    );

                scope.Complete();
            }
        }

        public void Dispose()
        {
            this.pool.Clear();
        }
    }
}
