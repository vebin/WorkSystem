namespace YmtSystem.Domain.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;
    using YmtSystem.CrossCutting.Utility;
    using YmtSystem.Domain.Repository;

    public class EFRepositoryContext : RepositoryContext, IEFRepositoryContext
    {
        private readonly ThreadLocal<YmatouDbContext> local = new ThreadLocal<YmatouDbContext>(() => new YmatouDbContext());

        public EFRepositoryContext()
        {

        }
        public override void RegisterDeleted<TAggregateRoot>(params TAggregateRoot[] obj)
        {
            //自动标记为删除状态
            if (obj.Length == 1)
                this.Context.Set<TAggregateRoot>().Remove(obj[0]);
            else
                this.Context.Set<TAggregateRoot>().RemoveRange(obj);
            Committed = false;
        }

        public override void RegisterModified<TAggregateRoot>(TAggregateRoot obj)
        {
            //标记为修改状态
            this.Context.Entry<TAggregateRoot>(obj).State = EntityState.Modified;
            Committed = false;
        }
        public override void RegisterNew<TAggregateRoot>(TAggregateRoot obj)
        {
            //自动标记为增加状态
            this.Context.Set<TAggregateRoot>().Add(obj);
            
            Committed = false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Committed)
                    Commit();
                this.Context.Dispose();
                this.local.Dispose();
                base.Dispose(disposing);
            }
        }

        protected override ExecuteResult<int> DoCommit(int retry = 0)
        {
            if (Committed) return ExecuteResult<int>.CreateFaile(0, "已提交");
            try
            {
                var result = this.Context.SaveChanges();
                this.Committed = true;
                return ExecuteResult<int>.CreateSuccess(result, "提交成功");
            }
            catch (Exception ex)
            {

                YmatouLoggingService.Warning("执行失败 {0}，重试 {1} 次", ex.ToString(), retry);
                //重试
                //return RetryUtility.RetryAction2(() =>
                // {
                //     var result = this.Context.SaveChanges();
                //     this.Committed = true;
                //     return Tuple.Create<ExecuteResult<int>, bool>(new ExecuteResult<int>(true, null, result), result > 0);
                // }, retry, 3000).Item1;  
                //RetryUtility.RetryAction2(() => System.Tuple.Create<ExecuteResult<int>, bool>(new ExecuteResult<int>(true, null, 0), true), 1000);
                return new ExecuteResult<int>(true, null, 0);
            }
        }

        protected override ExecuteResult<Task<int>> DoAsyncCommit(int retry = 0)
        {
            if (Committed) return ExecuteResult<Task<int>>.CreateFaile(Task.Factory.StartNew(() => 0), "已提交");
            try
            {
                var result = this.Context.SaveChangesAsync();
                Committed = true;
                return ExecuteResult<Task<int>>.CreateSuccess(result, "提交成功");
            }
            catch (Exception ex)
            {
                ExecuteResult<bool>.CreateFaile(false, ex.Message);
                return ExecuteResult<Task<int>>.CreateFaile(Task.Factory.StartNew(() => 0), ex.Message);
            }
        }

        public override void Rollback()
        {
            this.Context.ChangeTracker.Entries()
                              .Each(entry => entry.State = EntityState.Unchanged);
            Committed = false;
        }

        public DbContext Context
        {
            get { return local.Value; ; }
        }
    }
}
