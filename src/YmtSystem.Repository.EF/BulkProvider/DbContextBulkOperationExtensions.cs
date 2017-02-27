

namespace YmtSystem.Repository.EF.BulkProvider
{
    using System.Collections.Generic;
    using System.Data.Entity;
    internal static class DbContextBulkOperationExtensions
    {
        public static void BulkInsert<T>(this DbContext context, IEnumerable<T> entities, int batchSize = 1000)
        {
            var provider = new BulkOperationProvider(context);
            provider.Insert(entities, batchSize, System.Data.SqlClient.SqlBulkCopyOptions.Default);
        }
    }
}
