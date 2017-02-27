namespace YmtSystem.Repository.EF.BulkProvider
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public static class BulkInsertDefaults
    {
        public static int BatchSize = 5000;
        public static SqlBulkCopyOptions SqlBulkCopyOptions = SqlBulkCopyOptions.Default;
        public static int TimeOut = 30;
        public static int NotifyAfter = 1000;
    }

    public class BulkInsertOptions
    {
        public int BatchSize { get; set; }
        public SqlBulkCopyOptions SqlBulkCopyOptions { get; set; }

        /// <summary>
        /// Number of the seconds for the operation to complete before it times out
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// Callback event handler. Event is fired after n (value from NotifyAfter) rows have been copied to table where.
        /// </summary>
        public SqlRowsCopiedEventHandler Callback { get; set; }

        /// <summary>
        /// Number of rows after callback is fired.
        /// </summary>
        public int NotifyAfter { get; set; }

#if !NET40
        public bool EnableStreaming { get; set; }
#endif

        public BulkInsertOptions()
        {
            BatchSize = BulkInsertDefaults.BatchSize;
            SqlBulkCopyOptions = BulkInsertDefaults.SqlBulkCopyOptions;
            TimeOut = BulkInsertDefaults.TimeOut;
            NotifyAfter = BulkInsertDefaults.NotifyAfter;
        }
    }
}
