using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using NUnit.Framework;
using YmtSystem.Domain.Event;
using YmtSystem.Infrastructure.EventStore.Repository;
using YmtSystem.Infrastructure.EventStore.Repository.Register;

namespace YmtSystem.Infrastructure.Test
{

    [TestFixture]
    public class EventStoreTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            YmtSystem.CrossCutting.YmatouFramework.Start();
        }
        [Test]
        public void SaveEvent()
        {

            var eventRepository = new EventRepository();
            eventRepository.Add(new AAEvent
            {
                A = 100,
                B = 1000
            });
        }
    }

    public class EventRepository : MongodbEvnevntSourceRepository<AAEvent>, AAEventRepository
    {
    }
    public interface AAEventRepository : IMongodbEvnevntSourceRepository<AAEvent>
    {

    }
    public class AAEventMapping : RegisterEntityMappingConfigure
    {
        public AAEventMapping()
        {
            BsonClassMap.RegisterClassMap<AAEvent>(e =>
            {
                e.AutoMap();
                e.MapIdMember(_e => _e.EventKey);
            });
        }

        public override EntityMappingConfigure RegisterEntity()
        {
            Console.WriteLine("执行了。。");
            return new EntityMappingConfigure
            {
                ToDatabase = "EventSource",
                ToCollection = "AAEvent",
                MappType = typeof(AAEvent)
            };
        }
    }

    public class AAEvent : DomainEvent
    {
        public int A { get; set; }
        public int B { get; set; }
    }
}
