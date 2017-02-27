using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Xunit;
using Xunit.Extensions;
using YmtSystem.Repository.MongodbTest.Domain;
using YmtSystem.Repository.MongodbTest.Repository;
using YmtSystem.CrossCutting;

namespace YmtSystem.Repository.MongodbTest.Test
{
    public class UserTest
    {
        [Fact]
        public void AddOrder()
        {
            IOrderRepository orderRepo = new OrderRepository();
            orderRepo.Add(new Domain.Order
            {
                BuyerName = "zhangsan",
                dMoney = 10.1M,
                OrderId = Guid.NewGuid().ToString("N"),
                sType = "m"
            }, new WriteConcern { W=2});
        }
        [Fact]
        public void AddUserTest()
        {
            IUserRepository repo = new UserRepository();
            for (var i = 0; i < 2; i++)
            {
                var user = new User("u_" + i, "test00" + i, i % 2);
                user.AddUserAddress(new Address("中国", "上海", "上海市", "闸北,灵石路 xx", 123456, true));
                repo.Add(user);
            }
        }
        [Fact]
        public void CreateCappedConnection()
        {
            IUserRepository repo = new UserRepository();
            repo.CreateCappedCollection("test_c", "test_c1", 1000, 1000);
        }
        [Fact]
        public void UpdateUser()
        {
            var query = Query.EQ("_id", "u_1");
            var up = Update.Set("uName", "lisi");
            IUserRepository repo = new UserRepository();
            var result = repo.Update(query, up);
            Assert.Equal(true, result.Ok);
            Assert.Equal(1, result.DocumentsAffected);
        }
        [Fact]
        public void LinqToMonogo()
        {
            IUserRepository repo = new UserRepository();
            var result = repo.Find(u => u.UId == "u_0");

            Assert.Equal(1, result.Count());
        }
        [Fact]
        public void LinqToMonogoGroupBy()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                IUserRepository repo = new UserRepository();
                //mongo linq 不支持统计
                var result = repo.Find(u => u.UType == 0).GroupBy(e => e.UName);
                //延迟执行
                result.Each(g =>
                {
                    Console.WriteLine(g.Key + " " + g.Count());
                });
            });
        }
        [Fact]
        public void Group()
        {
            IUserRepository repo = new UserRepository();
            var result = repo.Group(new GroupArgs
            {
                KeyFields = GroupBy.Keys("type"),//设置了 KeyFields 则无需设置 KeyFunction
                Query = Query.GTE("cTime", DateTime.Now.AddDays(-3)),
                Initial = new BsonDocument("count", 0),
                ReduceFunction = "function(doc,per){return per.count+=1;}",
            }).ToArray();

            Assert.Equal(2, result.Length);
        }
        [Fact]
        public void MarpReduce()
        {
            //this 为当前文档，this.name 表示按name统计
            //相同name 计数 1
            var map = "function() { emit(this.uName,{count:1}) };";
            //迭代计算，相同name加1
            var reduce = @"function(key,emits){ 
                            var total=0; 
                            for(var i in emits){
                                total += emits[i].count;
                            }
                            return {count:total};
                        };";
            IUserRepository repo = new UserRepository();
            var result = repo.MapRedurce(new MapReduceArgs
             {
                 Query = Query.GTE("cTime", DateTime.Now.AddDays(-3)),
                 MapFunction = map,
                 ReduceFunction = reduce,
                 OutputCollectionName = "u_stats",
                 OutputMode = MapReduceOutputMode.Replace,//替换老的文档
             });
            Assert.Equal(true, result.Ok);
            //获取结果
            var resultDocument = result.GetResults();
        }
        [Fact]
        public void Aggregate()
        {
            IUserRepository repo = new UserRepository();
            var results = repo.Aggregation(new AggregateArgs
            {
                OutputMode = AggregateOutputMode.Cursor,
                Pipeline = new BsonDocument[] 
                {
                    //$group 表示分组统计
                    new BsonDocument("$group",new BsonDocument
                    {
                        //指定输出文档_id
                        {"_id","$uName"},
                        //指定输出文档 count 属性；$sum 内置统计关键词类似SQL sum
                        {"count",new BsonDocument("$sum",1)}
                    })
                    //, new BsonDocument("$out", "temp")
                }
            });
            var dictionary = new Dictionary<string, int>();
            //遍历结果
            foreach (var result in results)
            {
                var x = result["_id"].AsString;
                var count = result["count"].AsInt32;
                Console.WriteLine(x + " -> " + count);
                dictionary[x] = count;
            }
        }
    }
}
