using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using YmtSystem.Repository.EF.BulkProvider.Metadata;

namespace YmtSystem.Repository.EF.BulkProvider
{
    internal class BulkOperationProvider
    {
        private DbContext _context;
        private string _connectionString;

        public BulkOperationProvider(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            _context = context;
            _connectionString = context.Database.Connection.ConnectionString;
        }

        public void Insert<T>(IEnumerable<T> entities, int? batchSize,SqlBulkCopyOptions options)
        {
            using (var dbConnection = new SqlConnection(_connectionString))
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        var _options = new BulkInsertOptions { SqlBulkCopyOptions = options };
                        if (batchSize.HasValue)
                            _options.BatchSize = batchSize.Value;

                        Insert(entities, transaction, _options, batchSize);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        throw;
                    }
                }
            }
        }

        private void Insert<T>(IEnumerable<T> entities, SqlTransaction transaction, BulkInsertOptions options, int? batchSize)
        {
           
            using (var reader = new MappedDataReader<T>(entities, _context))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(transaction.Connection, options.SqlBulkCopyOptions, transaction))
                {
                    var keepIdentity = (SqlBulkCopyOptions.KeepIdentity & options.SqlBulkCopyOptions) > 0;
                    sqlBulkCopy.BulkCopyTimeout = options.TimeOut;
                    sqlBulkCopy.BatchSize = options.BatchSize;
                    sqlBulkCopy.DestinationTableName = string.Format("[{0}].[{1}]", reader.SchemaName, reader.TableName);
#if !NET40
                    sqlBulkCopy.EnableStreaming = options.EnableStreaming;
#endif
                    sqlBulkCopy.NotifyAfter = options.NotifyAfter;
                    if (options.Callback != null)
                    {
                        sqlBulkCopy.SqlRowsCopied += options.Callback;
                    }

                    foreach (var kvp in reader.Cols)
                    {
                        if (kvp.Value.IsIdentity && !keepIdentity)
                        {
                            continue;
                        }
                        sqlBulkCopy.ColumnMappings.Add(kvp.Value.ColumnName, kvp.Value.ColumnName);
                    }


                    sqlBulkCopy.WriteToServer(reader);
                }
            }
        }      
    }
}
