namespace YmtSystem.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.DependencyResolution;
    using System.Data.Entity.Infrastructure.Interception;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;

    public class SqlCommandInterceptorService
    {
        public static void Start(string contextName)
        {
            if (string.IsNullOrEmpty(contextName)) return;
            if (DbConfigure.GetConfigure(contextName).MonitorSqlCommond)
                DbInterception.Add(new SqlCommandInterceptor(contextName));
        }
    }

    internal class SqlCommandInterceptor : IDbCommandInterceptor
    {
        private Stopwatch watch;
        private DbConfigure cfg;
        public SqlCommandInterceptor(string contextName)
        {
            watch = new Stopwatch();
            cfg = DbConfigure.GetConfigure(contextName);
        }

        public void NonQueryExecuting(
            DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            //Log(command, interceptionContext, "NonQueryExecuting");
            watch.Restart();
        }

        public void NonQueryExecuted(
            DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            watch.Stop();
            var total = watch.Elapsed.TotalSeconds;
            if (total >= cfg.MonitorSlowSqlRunTime)
                Log(command, interceptionContext, "NonQueryExecuting", string.Format("run {0}'s", total));
        }

        public void ReaderExecuting(
            DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            watch.Restart();
            //Log(command, interceptionContext, "ReaderExecuting");
        }

        public void ReaderExecuted(
            DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            watch.Stop();
            var total = watch.Elapsed.TotalSeconds;
            if (total >= cfg.MonitorSlowSqlRunTime)
                Log(command, interceptionContext, "ReaderExecuted", string.Format("run {0}'s", total));
        }

        public void ScalarExecuting(
            DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            watch.Restart();
            //Log(command, interceptionContext, "ScalarExecuting");
        }

        public void ScalarExecuted(
            DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            watch.Stop();
            var total = watch.Elapsed.TotalSeconds;
            if (total >= cfg.MonitorSlowSqlRunTime)
                Log(command, interceptionContext, "ScalarExecuted", string.Format("run {0}'s", total));
        }

        private void Log<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext, string sqlType = null, string desc = null)
        {
            if (interceptionContext.Exception != null)
            {
                YmatouLoggingService.Error("sql {0}，{1}， {2} -> failed with exception {3}", sqlType, desc,
                    command.CommandText.Replace(Environment.NewLine, ""), interceptionContext.Exception);
            }
            else
            {
                YmatouLoggingService.Info("sql {0} ，{1}，{2}", sqlType, desc, command.CommandText.Replace(Environment.NewLine, ""));
            }
        }
    }
}
