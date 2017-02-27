namespace YmtSystem.Repository.NH
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using NHibernate;
    using NHibernate.SqlCommand;
    using YmtSystem.CrossCutting;

    public class SqlCommandInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Trace.WriteLine(sql.ToString());
            YmtSystem.CrossCutting.YmatouLoggingService.Info(sql.ToString());
            return base.OnPrepareStatement(sql);
        }
    }
}
