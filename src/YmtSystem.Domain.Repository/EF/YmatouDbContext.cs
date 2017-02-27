namespace YmtSystem.Domain.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;
    using YmtSystem.CrossCutting.Utility;
    using YmtSystem.Domain.Repository.EF.ModelMapping;
    using YmtSystem.Domain.Shard;

    public sealed class YmatouDbContext : DbContext
    {
        public YmatouDbContext()
            : base(@"Data Source=YMT-LIGUO\LIGUO;Initial Catalog=Ymt_Test_6;Persist Security Info=True;User ID=sa;Password=123@#$123asd")
        {
            //
            //链接字符串里设置timeOut
            //Data Source=192.168.1.230;Initial Catalog=Ymt_Test_Lg;Persist Security Info=True;User ID=ymt_admin;Password=admin@ymatou.com
            //local db:
            //
            //this.Database.CommandTimeout = 30; //30s 
            //this.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }

        public override int SaveChanges()
        {
            TrackerEntityStatusChang();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            TrackerEntityStatusChang();
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //BuildManagerWrapper.Current.PublicTypes.AsParallel().Where(e => e != null && !e.FullName.Contains("IDBMappingRegister") && typeof(IDBMappingRegister).IsAssignableFrom(e)).Each(e => ((IDBMappingRegister)Activator.CreateInstance(e)).Register(modelBuilder.Configurations));            
            EntityRegisteService.Registe(modelBuilder.Configurations);
            base.OnModelCreating(modelBuilder);
        }
        private void SetEntityStatus(DbEntityEntry<IEntityExtend> e)
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
                case EntityState.Deleted:
                    e.Entity.IsDelete = true;
                    e.Entity.ModifyTime = DateTime.Now;
                    base.Entry(e.Entity).State = EntityState.Modified;
                    break;
            }
        }
        private void TrackerEntityStatusChang()
        {
            var entityStatusTracker = ChangeTracker.Entries<IEntityExtend>();
            entityStatusTracker.Each(e =>
            {
                SetEntityStatus(e);
            });

            //var entityVersionTracker = ChangeTracker.Entries<IEntityExtend>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            //entityVersionTracker.Each(e =>
            //{
            //    e.Entity.Version += 1;
            //});
        }
    }
}
