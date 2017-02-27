using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace YmtSystem.Infrastructure.EventStore
{
    public class MongoDbDatabaseServer : IDisposable
    {
        [ThreadStatic]
        private static MongoServer server;
        private MongoDatabase db;

        public MongoDbDatabaseServer()
        {
            if (server == null)
                server = new MongoClient(MongoClientSettings.FromUrl(MongoUrl.Create(Configure.MongoUrl))).GetServer();
            server.Connect(TimeSpan.FromSeconds(30));
            db = server.GetDatabase(Configure.MongoDatabaseName);
        }

        public MongoCollection<T> GetTypeCollection<T>(string collectionName)
        {
            return db.GetCollection<T>(collectionName);
        }
        public MongoCollection GetCollection(string collectionName)
        {
            return db.GetCollection(collectionName);
        }
        public MongoCollection<T> GetTypeCollection<T>(string database, string collectionName)
        {
            return server.GetDatabase(database).GetCollection<T>(collectionName);
        }
        public MongoCollection GetCollection(string database, string collectionName)
        {
            return server.GetDatabase(database).GetCollection(collectionName);
        }
        public void Stop()
        {
            server.Disconnect();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
