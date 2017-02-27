namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;
    using YmtSystem.CrossCutting.Utility;
    using YmtSystem.Domain.Shard;
    using YmtSystem.Repository.EF.ModelMapping;
    using YmtSystem.Infrastructure.Config;
    using YmtSystem.Repository.EF.Factory;

    /// <summary>
    ///  数据库上下文
    /// </summary>
    public abstract class YmtSystemDbContext : DbContext
    {
        public YmtSystemDbContext(string storingContextName, string dbFileName = "db.config")
            : base(
                DbConnectionFactory.Builder(storingContextName, dbFileName),
                DbConfigure.GetConfigure(storingContextName, dbFileName).ContextOwnsConnection)
        {
            //Database.SetInitializer<YmtSystemDbContext>(null);
            //YmatouLoggingService.Debug("YmtSystemDbContext create ok...thread id {0},contextName {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, storingContextName);
        }
        /// <summary>
        /// 初始化上下文
        /// </summary>
        public YmtSystemDbContext()
            : this(string.Empty)
        {

        }

        /// <summary>
        /// 同步保存修改
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            TrackerEntityStatusChang();
            return base.SaveChanges();
        }

        /// <summary>
        /// 异步保存修改
        /// </summary>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync()
        {
            TrackerEntityStatusChang();
            return base.SaveChangesAsync();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //可以移除在 System.Data.Entity.ModelConfiguration.Conventions 命名空间中定义的任何约定
        //    //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    EntityRegisteService.Registe(modelBuilder.Configurations);
        //    modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        //    base.OnModelCreating(modelBuilder);
        //}

        /// <summary>
        /// 根据实体状态设置创建，修改时间
        /// </summary>
        /// <param name="e"></param>
        private void SetEntityCreateOrModifyTime(DbEntityEntry<IEntityExtend> e)
        {
            switch (e.State)
            {
                case EntityState.Added:
                    e.Entity.CreateTime = DateTime.Now;
                    e.Entity.ModifyTime = DateTime.Now;
                    break;
                case EntityState.Modified:
                    e.Entity.ModifyTime = DateTime.Now;
                    break;
            }
        }

        /// <summary>
        /// 跟踪实体状态修改
        /// </summary>
        private void TrackerEntityStatusChang()
        {
            ChangeTracker.Entries<IEntityExtend>().Each(SetEntityCreateOrModifyTime);
            ChangeTracker.Entries<IEntityLogicDelete>().Where(e => e.State == EntityState.Deleted)/*.OfType<IEntityLogicDelete>()*/.Each(e =>
            {
                e.Entity.IsDelete = true;
                base.Entry(e.Entity).State = EntityState.Modified;
            });
        }
    }
}
