using System.Data.SqlClient;

namespace YmtSystem.Repository.EF
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using YmtSystem.CrossCutting;
    using YmtSystem.Repository.EF.BulkProvider;


    /// <summary>
    /// entity framework EFUnitOfWork   
    /// </summary>
    public class EFUnitOfWork : YmtSystemDbContext, IEFUnitOfWork
    {
        //private YmtSystemDbContext _context { get { return LocalServiceLocator.GetService<YmtSystemDbContext>(); } }

        public EFUnitOfWork(string storingContextName, string dbFileName = "db.config")
            : base(storingContextName, dbFileName)
        {

        }

        public void RegisterDeleted<TEntity>(params TEntity[] obj)
             where TEntity : class
        {
            //自动标记为删除状态
            if (obj.Length == 1)
                this.CreateSet<TEntity>().Remove(obj[0]);
            else
                this.CreateSet<TEntity>().RemoveRange(obj);
        }

        public void RegisterModified<TEntity>(TEntity obj)
              where TEntity : class
        {
            //标记为修改状态          
            base.Entry<TEntity>(obj).State = EntityState.Modified;
        }

        public void RegisterNew<TEntity>(TEntity obj)
              where TEntity : class
        {
            //自动标记为增加状态  
            this.Context.Configuration.AutoDetectChangesEnabled = true;
            this.CreateSet<TEntity>().Add(obj);
//            YmatouLoggingService.Debug("TEntity {0} state {1}", typeof (TEntity).FullName,
//                base.Entry<TEntity>(obj).State);
        }

        public void RegisterNew<TEntity>(IEnumerable<TEntity> obj)
              where TEntity : class
        {
            //实体变更检查大数据插入时会影响性能          
            this.CreateSet<TEntity>().AddRange(obj);
        }

        public void RegisterNewBulk<TEntity>(IEnumerable<TEntity> obj) where TEntity : class
        {
            this.Context.Configuration.AutoDetectChangesEnabled = false;
            this.Context.Configuration.ValidateOnSaveEnabled = false;
            this.CreateSet<TEntity>();
            this.Context.BulkInsert(obj);
        }

        public ExecuteResult<int> Commit(int retry = 0, bool lockd = false)
        {
            var tmpRetry = retry;
            var fail = false;
            var result = 0;
            do
            {
                try
                {
                    result = base.SaveChanges();
                    fail = false;
                    YmatouLoggingService.Debug("Db Context Commit {0}", result);
                    return ExecuteResult<int>.CreateSuccess(result, "提交成功");
                }
                catch (DbEntityValidationException ex)
                {
                    ex.EntityValidationErrors.Each(_e =>
                    {
                        var errs = _e.Entry.GetValidationResult().ValidationErrors.ValidationErrorAppendToString();
                        YmatouLoggingService.Error("entity：{0}，Validation fail：{1}", _e.Entry.Entity.GetType().Name,
                            errs);
                    });
                    return ExecuteResult<int>.CreateFaile(0, "Db Context Commit fail");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    fail = true;
                    ex.Entries
                        .Each(entry =>
                        {
                            if (entry != null)
                            {
                                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                                {
                                    //Context中实体的值覆盖db中实体的值
                                    var currentVal = entry.CurrentValues; //当前值
                                    var dbVal = entry.GetDatabaseValues();
                                    //获取数据库中的值                                            
                                    entry.OriginalValues.SetValues(dbVal);
                                    //entry.CurrentValues.SetValues(dbVal.Clone());
                                    entry.State = EntityState.Modified;
                                }
                                else
                                {
                                    //数据库中的覆盖entity中的值
                                    entry.Reload();
                                }
                            }
                        }, errorHandler: err =>
                        {
                            YmatouLoggingService.Error("重试异常 {0}", err.ToString());
                        });
                    var _ex = ex.GetBaseException() as SqlException;                     
                    if (_ex != null && (_ex.Number == 2627 || _ex.Number == 515))
                        YmatouLoggingService.Debug("EFUnitOfWork DbUpdateConcurrencyException repeat insert,{0}",
                            _ex.Message);
                    else
                        YmatouLoggingService.Error(
                            "EFUnitOfWork DbUpdateConcurrencyException entity : {0}，err:{1}，重试 {2} 次",
                            ex.Entries.TrySerializeEntity(),
                            ex.ToString(), tmpRetry);
                }
//                catch (DbUpdateException ex)
//                {
//                    fail = true;
//                    result = -1;
//                    var _ex = ex.GetBaseException() as SqlException; 
//                    if (_ex != null && (_ex.Number == 2627 || _ex.Number == 515))
//                        YmatouLoggingService.Debug("EFUnitOfWork DbUpdateException repeat insert,{0}", _ex.Message);
//                    else
//                        YmatouLoggingService.Error("EFUnitOfWork DbUpdateException entity : {0}，err:{1}，重试 {2} 次",
//                            ex.Entries.TrySerializeEntity(),
//                            ex.ToString(), tmpRetry);
//                    throw;
//                }
            } while (fail && tmpRetry-- > 0);
            return new ExecuteResult<int>(fail == false, fail == false ? "执行成功" : "更新失败", result);
        }

        public async Task<ExecuteResult<int>> AsyncCommit(int retry = 0)
        {
            var tmpRetry = retry;
            bool fail;
            var result = 0;
            do
            {
                try
                {
                    result = await base.SaveChangesAsync().ConfigureAwait(false);
                    fail = false;
                    return await Task.Factory.StartNew(() => ExecuteResult<int>.CreateSuccess(result, "已提交"));
                }
                catch (DbEntityValidationException ex)
                {
                    ex.EntityValidationErrors.Each(_e =>
                    {
                        var errs = _e.Entry.GetValidationResult().ValidationErrors.ValidationErrorAppendToString();
                        YmatouLoggingService.Error("实体：{0}，验证错误：{1}", _e.Entry.Entity.GetType().Name, errs);
                    });
                    return ExecuteResult<int>.CreateFaile(0, "提交失败");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    fail = true;
                    ex.Entries.Each(entry =>
                        {
                            if (entry != null)
                            {
                                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                                {
                                    //Context中实体的值覆盖db中实体的值
                                    var currentVal = entry.CurrentValues;  //当前值
                                    var dbVal = entry.GetDatabaseValues(); //获取数据库中的值                                            
                                    entry.OriginalValues.SetValues(dbVal);
                                    entry.CurrentValues.SetValues(dbVal.Clone());
                                    entry.State = EntityState.Modified;
                                }
                                else
                                {
                                    //数据库中的覆盖entity中的值
                                    entry.Reload();
                                }
                            }
                        });
                    var _ex = ex.GetBaseException() as SqlException; 
                    if (_ex != null && (_ex.Number == 2627 || _ex.Number == 515))
                        YmatouLoggingService.Debug("EFUnitOfWork DbUpdateConcurrencyException  repeat insert,{0}", _ex.Message);
                    else
                        YmatouLoggingService.Error("EFUnitOfWork DbUpdateConcurrencyException entity : {0}，err:{1}，重试 {2} 次", ex.Entries.TrySerializeEntity(),
                            ex.ToString(), tmpRetry);
                }
//                catch (DbUpdateException ex)
//                {
//                    fail = true;
//                    var _ex = ex.GetBaseException() as SqlException; 
//                    if (_ex != null && (_ex.Number == 2627 || _ex.Number == 515))
//                        YmatouLoggingService.Debug("EFUnitOfWork DbUpdateException  repeat insert,{0}", _ex.Message);
//                    else
//                        YmatouLoggingService.Error("EFUnitOfWork DbUpdateException entity : {0}，err:{1}，重试 {2} 次",
//                            ex.Entries.TrySerializeEntity(),
//                            ex.ToString(), tmpRetry);
//                }
            } while (fail && tmpRetry-- > 0);
            return await Task.Factory.StartNew(() => new ExecuteResult<int>(fail == false, fail == false ? "执行成功" : "提交失败", result));
        }

        public void RollbackChanges()
        {
            base.ChangeTracker.Entries()
                              .Each(entry => entry.State = EntityState.Unchanged);
        }

        [DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public DbContext Context { get { return this; } }

        public DbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
