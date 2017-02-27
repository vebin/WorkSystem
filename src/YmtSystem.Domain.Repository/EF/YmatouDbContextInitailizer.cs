namespace YmtSystem.Domain.Repository.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;


    public class YmatouDbContextInitailizer
    {
        public static void Init()
        {
            //Database.SetInitializer<YmatouDbContext>(new MigrateDatabaseToLatestVersion<YmatouDbContext, Configuration>());
            //Database.SetInitializer<YmatouDbContext>(new YmatouDbContextCreateInitializer());
            //Database.SetInitializer<YmatouDbContext>(new YmatouDbContextDropCreateDatabaseIfModelChangesInitailizer());
            Database.SetInitializer<YmatouDbContext>(new CreateAndMigrateDatabaseInitializer<YmatouDbContext, Configuration<YmatouDbContext>>());
        }
    }
  
    /// <summary>
    /// 如实体模型改变，则删除数据库，重新创建数据库。
    /// <remarks>这个策略不适合线上环境或者数据库已有大量数据</remarks>
    /// </summary>
    class YmatouDbContextDropCreateDatabaseIfModelChangesInitailizer : DropCreateDatabaseIfModelChanges<YmatouDbContext>
    {
        protected override void Seed(YmatouDbContext context)
        {

            base.Seed(context);
        }
    }

    /// <summary>
    /// 如果数据不存储，则自动创建
    /// <remarks>这个策略适合第一次使用</remarks>
    /// </summary>
    class YmatouDbContextCreateInitializer : CreateDatabaseIfNotExists<YmatouDbContext>
    {
        protected override void Seed(YmatouDbContext context)
        {
            // 按需要创建索引等初始化脚本
            //context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_CUSTOMER_USERNAME ON Customers(UserName)");
            //如果数据不存储，则自动创建
            context.Database.CreateIfNotExists();
            base.Seed(context);
        }
    }
}
