namespace YmtSystem.Repository.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// 数据库上初始化
    /// </summary>
    public class YmtSystemDbContextInitailizer
    {
        public static void Init()
        {
            //Database.SetInitializer<YmatouDbContext>(new MigrateDatabaseToLatestVersion<YmatouDbContext, Configuration>());
            //Database.SetInitializer<YmatouDbContext>(new YmatouDbContextCreateInitializer());
            //Database.SetInitializer<YmatouDbContext>(new YmatouDbContextDropCreateDatabaseIfModelChangesInitailizer());
            //Database.SetInitializer<YmtSystemDbContext>(new CreateAndMigrateDatabaseInitializer<YmtSystemDbContext, Configuration<YmtSystemDbContext>>());
            Database.SetInitializer<YmtSystemDbContext>(null);
        }
    }

    /// <summary>
    /// 如实体模型改变，则删除数据库，重新创建数据库。
    /// <remarks>这个策略不适合线上环境或者数据库已有大量数据</remarks>
    /// </summary>
    class YmatouDbContextDropCreateDatabaseIfModelChangesInitailizer : DropCreateDatabaseIfModelChanges<YmtSystemDbContext>
    {
        protected override void Seed(YmtSystemDbContext context)
        {

            base.Seed(context);
        }
    }

    /// <summary>
    /// 如果数据不存储，则自动创建
    /// <remarks>这个策略适合第一次使用</remarks>
    /// </summary>
    class YmatouDbContextCreateInitializer : CreateDatabaseIfNotExists<YmtSystemDbContext>
    {
        protected override void Seed(YmtSystemDbContext context)
        {
            // 按需要创建索引等初始化脚本
            //context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_CUSTOMER_USERNAME ON Customers(UserName)");
            //如果数据不存储，则自动创建
            context.Database.CreateIfNotExists();
            base.Seed(context);
        }
    }
}
