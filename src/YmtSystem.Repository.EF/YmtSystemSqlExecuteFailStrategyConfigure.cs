namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class YmtSystemDbConfiguration : DbConfiguration
    {
        public YmtSystemDbConfiguration() {
        }
        public YmtSystemDbConfiguration(string contextName)
        {
            //自动恢复连接失败时的事务
            var cfg = DbConfigure.GetConfigure(contextName);
            if (cfg.TransactionFailRetry)
                SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());
            //由于网络因素可能造成连接异常，采用的重试策略。
            //1.DefaultExecutionStrategy:默认策略，除了SQL数据库，其他数据库不执行任何重试操作
            //2.SqlAzureExecutionStrategy：这个执行策略继承至 DbExecutionStrategy，它会在 Sql Azure 遇到短暂错误时进行重试操作
            //3.DbExecutionStrategy：这个类是一个比较适合用来实现自定义执行策略的基类，它实现了一个指数重试的策略，即，第一次尝试连接时等待为零，后续等待时间以指数级进行增长，直到达到最大尝试次数才终止。这个类有一个抽象方法 ShouldRetryOn ，子类在实现它时，自己决定哪些个异常需要进行重试操作
            //4.DefaultSqlExecutionStrategy：这是一个内部默认使用的执行策略，这个策略也不会进行任何的重试操作，但是，它会将这些连接错误的异常包裹起来，并通知用户可能需要开启韧性连接。
            //http://msdn.microsoft.com/en-us/library/dn456835.aspx
            if (cfg.ConnectionFailRetry)
                SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlAzureExecutionStrategy(2, TimeSpan.FromSeconds(30)));
        }


    }

    public class YmtSystemSqlExecuteFailStrategyService
    {
        public static void Start(string contextName)
        {
            new YmtSystemDbConfiguration(contextName);
        }
    }
}
