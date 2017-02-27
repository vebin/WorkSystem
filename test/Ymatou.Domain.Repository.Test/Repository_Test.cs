using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using YmtSystem.CrossCutting;
using NUnit.Framework;
using YmtSystem.Repository.EF;
using YmtSystem.Domain.Repository;
using YmtSystem.Domain.Shard.Validator;
using YmtSystem.Domain.Shard;
using YmtSystem.Infrastructure.AutoMapperAdapter;
using Ymt.Trading.Domain.Model.Order;
using Ymt.Trading.Domain.Repository;
using Ymt.Trading.Domain.Model.User;
using Ymt.Trading.Domain.Model.Table;
using System.Threading;
using System.Diagnostics;
using YmtSystem.Repository.EF.Factory;
using Ymt.Trading.Domain.Repository.TradingContext;

namespace YmtSystem.Domain.Repository.Test
{
    [TestFixture]
    public class Repository_Test
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            new TraceSource("YmtSystem").TraceEvent(TraceEventType.Warning, (int)TraceEventType.Information, "启动");
            YmatouFramework.Start();
        }
        [TestFixtureTearDown]
        public void Stop()
        {
            YmatouFramework.Stop();
        }
        [Test]
        [Description("队列测试")]
        public void Queue()
        {
            System.Collections.Concurrent.ConcurrentQueue<int> pool = new System.Collections.Concurrent.ConcurrentQueue<int>();
            pool.Enqueue(1);
            pool.Enqueue(2);
            pool.Enqueue(3);

            while (true)
            {
                int tmp;
                if (pool.TryDequeue(out tmp))

                    Console.WriteLine(tmp);
                else break;
            }
        }
        [Test]
        [Description("多个unitofwork提交")]
        public void Multiple_UnitofWorkCommit()
        {
            Action work1 = () =>
            {
                var t14Repo = LocalServiceLocator.GetService<IT14Repository>();
                t14Repo.Add(new T14(Guid.NewGuid().ToString("N"), "ok"));
                t14Repo.Context.Commit();
                //return t14Repo.Context;
            };

            Action work2 = () =>
           {
               var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
               var order = new YmtOrder("aacd0bc4-34e5-4c0f-a255-214d567698cd", "test_order", 77);
               order.AddOrderLine(new OrderLine(100))
               .AddOrderLine(new OrderLine(120))
               .AddOrderLine(new OrderLine(150))
               .SetAddress(new ReceiptAddress { City = "A", Zip = "B", Street = "C" });
               var verify = order.Validator();
               Console.WriteLine(verify.Message);
               Assert.AreEqual(true, verify.Success);
               orderRepository.AddOrUpdate(e => e.Id, order);
               orderRepository.Context.Commit();
               //return orderRepository.Context;
           };
            MultipleUnitOfWorkBuilder.Instance.Commit(work1, work2);
            //using (MultipleUnitOfWorkBuilder.Instance.Append(work1).Append(work2).Commit())
            //{
            //};
        }
        [Test]
        [Description("逻辑删除")]
        public void Logic_delete()
        {

            var t14Repo = LocalServiceLocator.GetService<IT14Repository>();
            var t14Model = t14Repo.FindOne<int>(100005);
            Assert.AreEqual(100005, t14Model.TId);

            t14Repo.Add(new T14(Guid.NewGuid().ToString("N"), "ok"));
            t14Repo.Context.Commit();

            //t14Repo.Remove(t14Model);
            //t14Repo.Context.Commit();

            // t14Repo.ExecuteCommand("delete from [dbo].[T_14] where id ='6e47ecd7dae146c19663eb907f661700' ");

            var tRepository = LocalServiceLocator.GetService<ITRepository>();
            var t = tRepository.FindOne<string>("00023f5c5ce74773a49caf4b3ffbb052");
            tRepository.Remove(t);
            var result = tRepository.Context.Commit().ResultData;
            Assert.AreEqual(1, result);
        }
        [Test]
        [Description("批量插入")]
        public void Bulk_Insert()
        {
            var list1 = new List<T14>();
            for (var i = 0; i < 50000; i++)
            {
                var t = new T14(Guid.NewGuid().ToString("N"), "JJ");

                var validatorResult = t.Validator();
                Assert.AreEqual(true, validatorResult.ResultData.Success);
                list1.Add(t);
            }
            var watch1 = Stopwatch.StartNew();
            var t14Repository = LocalServiceLocator.GetService<IT14Repository>();
            t14Repository.Add(list1, true);
            t14Repository.Context.Commit();
            Console.WriteLine("耗时：{0} 秒", watch1.Elapsed.TotalSeconds);
            return;
            var list = new List<T1>();
            for (var i = 0; i < 50000; i++)
            {
                var t = new T1(Guid.NewGuid().ToString("N"), 100, DateTime.Now, "AAA");

                var validatorResult = t.Validator();
                Assert.AreEqual(true, validatorResult.ResultData.Success);
                list.Add(t);
            }
            var watch = Stopwatch.StartNew();
            var tRepository = LocalServiceLocator.GetService<ITRepository>();
            tRepository.Add(list);
            tRepository.Context.Commit();
            Console.WriteLine("耗时：{0} 秒", watch.Elapsed.TotalSeconds);
        }
        [Test]
        [Description("统计测试")]
        public void Ef_Select_partField()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var sumPrice = orderRepository.FindAll().Select(e => e.Price).AsEnumerable().ToList();
            //Console.WriteLine(sumPrice);
        }
        [Test]
        [Description("统计测试")]
        public void Ef_Stats_Group()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var sumPrice = orderRepository.Group(Specifications.AnySpecification<YmtOrder>.CreateAnySpecification().SatisfiedBy(), e => e.Price).Select(e => e.Key);
            sumPrice.Each(e => Console.WriteLine(e));
            //Console.WriteLine(sumPrice);
        }
        [Test]
        [Description("统计测试")]
        public async void Ef_Stats_AsyncCount()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var sumPrice = await orderRepository.CountAsync(Specifications.AnySpecification<YmtOrder>.CreateAnySpecification().SatisfiedBy());
            Console.WriteLine(sumPrice);
        }
        [Test]
        [Description("统计测试")]
        public async void Ef_Stats_AsyncSum()
        {
            var stopwatch = Stopwatch.StartNew();
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var sumPrice = await orderRepository.SumAsync(Specifications.AnySpecification<YmtOrder>.CreateAnySpecification().SatisfiedBy(), e => e.Price);
            Console.WriteLine("结果:{0},耗时:{1} 毫秒", sumPrice, stopwatch.Elapsed.TotalMilliseconds);
        }
        [Test]
        [Description("并行统计测试")]
        public void Ef_Stats_ParallelSum()
        {
            //var stopwatch = Stopwatch.StartNew();
            //var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            //var sumPrice = orderRepository.AsyncSum<decimal>(Specifications.AnySpecification<YmtOrder>.CreateAnySpecification().SatisfiedBy()
            //    , e => e.Price, e => e);
            //Console.WriteLine("结果:{0},耗时:{1} 毫秒", sumPrice, stopwatch.Elapsed.TotalMilliseconds);
        }
        [Test]
        [Description("统计测试")]
        public void Ef_Stats_SyncSum()
        {
            var stopwatch = Stopwatch.StartNew();
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var sumPrice = orderRepository.SumSync(Specifications.AnySpecification<YmtOrder>.CreateAnySpecification().SatisfiedBy(), e => e.Price);
            Console.WriteLine("结果:{0},耗时:{1} 毫秒", sumPrice, stopwatch.Elapsed.TotalMilliseconds);
        }
        [Test]
        [Description("统计测试")]
        public void Ef_Stats_Count()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var sumPrice = orderRepository.Count(Specifications.AnySpecification<YmtOrder>.CreateAnySpecification().SatisfiedBy());
            Console.WriteLine(sumPrice);
        }
        [Test]
        [Description("执行SQL")]
        public void ExecuteSql()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            orderRepository.ExecuteCommand("  update  [ymt_trading_order_2] set Price=@price where id =@id"
                , new SqlParameter("@price", "1000")
                , new SqlParameter("@id", "aacd0bc4-34e5-4c0f-a255-214d567698cd"));
        }
        [Test]
        [Description("添加或修改")]
        public void AddOrUpdateOrder()
        {
            var orderRepository = new YmtOrderRepository(new EFUnitOfWork("orderContext"));//LocalServiceLocator.GetService<IYmtOrderRepository>();
            var order = new YmtOrder("0000b222-db89-4c4b-b226-1fc694d3d065", "test order", 60);

            //
            order.AddOrderLine(new OrderLine(100))
            .AddOrderLine(new OrderLine(120))
            .AddOrderLine(new OrderLine(150))
            .SetAddress(new ReceiptAddress { City = "A", Zip = "B", Street = "C" });
            var verify = order.Validator();
            Console.WriteLine(verify.Message);
            Assert.AreEqual(true, verify.Success);
            orderRepository.AddOrUpdate(e => e.Id, order);
            orderRepository.Context.Commit();
        }
        [Test]
        [Description("并发测试")]
        public void ConcurrentTest_AdoNet()
        {
            var listTask = new List<Task>();
            var th = new Task(() =>
           {
               using (var conn = new SqlConnection(@"Data Source=YMT-LIGUO\LIGUO;Initial Catalog=Ymt_Test_10;Persist Security Info=True;User ID=sa;Password=123@#$123asd"))
               {
                   conn.Open();
                   var comm = conn.CreateCommand();
                   comm.CommandType = System.Data.CommandType.Text;
                   comm.CommandText = "update [ymt_trading_order_2] set price=price+10";
                   var result = comm.ExecuteNonQuery();
               }
           });
            var th2 = new Task(() =>
               {
                   using (var conn = new SqlConnection(@"Data Source=YMT-LIGUO\LIGUO;Initial Catalog=Ymt_Test_10;Persist Security Info=True;User ID=sa;Password=123@#$123asd"))
                   {
                       conn.Open();
                       var comm = conn.CreateCommand();
                       comm.CommandType = System.Data.CommandType.Text;
                       comm.CommandText = "update [ymt_trading_order_2] set price=price+10";
                       var result = comm.ExecuteNonQuery();
                   }
               });

            listTask.Add(th);
            listTask.Add(th2);
            listTask.Each(e => e.Start());
            Task.WaitAll(listTask.ToArray());
        }
        [Test]
        [Description("并发测试")]
        public void Concurrent_Query_Test()
        {
            if (YmatouFramework.Status != YmatouFrameworkStatus.Started)
                throw new System.InvalidOperationException("框架未启动");

            var task1 = new Task(() =>
            {
                var order = OrderRepo(orderRepository => orderRepository.Exists(e => e.Id == "0000b222-db89-4c4b-b226-1fc694d3d065"));
                //using (var unitOfWork = LocalServiceLocator.GetService<IEFUnitOfWork>("order"))
                //{
                //    var orderRepository = LocalServiceLocator.GetService<IRepositoryFactory>().CreateRepository<YmtOrder, YmtOrderRepository>(unitOfWork);
                //    var b = orderRepository.Exists(e => e.Id == "0000b222-db89-4c4b-b226-1fc694d3d065");
                //    Console.WriteLine("找到订单1");
                //}
                Console.WriteLine("找到订单1 {0}", order);
            });
            var task2 = new Task(() =>
            {
                var order = OrderRepo(orderRepository => orderRepository.Exists(e => e.Id == "0000e3a4-c40c-4f18-ae90-26a206941ceb"));
                //using (var unitOfWork = LocalServiceLocator.GetService<IDbContextFactory>().CreateContext<YmtTradingUnitOfwork>())
                //{
                //    var orderRepository = LocalServiceLocator.GetService<IRepositoryFactory>().CreateRepository<YmtOrder, YmtOrderRepository>(unitOfWork);
                //    var b = orderRepository.Exists(e => e.Id == "0000e3a4-c40c-4f18-ae90-26a206941ceb");
                //    Console.WriteLine("找到订单2");
                //}
                Console.WriteLine("找到订单2 {0}",order);
            });

            task1.Start();
            task2.Start();

            Task.WaitAll(task1, task2);
        }
        [Test]
        [Description("并发测试")]
        public void ConcurrentTest()
        {
            if (YmatouFramework.Status != YmatouFrameworkStatus.Started) throw new System.InvalidOperationException("框架未启动");
            var listTask = new List<Task>();
            {
                var th = new Task(() =>
                {
                    YmtOrder tmpOrder = null;

                    try
                    {
                        //new YmtOrderRepository(new EFUnitOfWork("orderContext")); //
                        OrderRepo(orderRepository =>
                        {
                            tmpOrder = orderRepository.FindOne("0000b222-db89-4c4b-b226-1fc694d3d065");
                            //tmpOrder.Name = string.Format("并发测试{0}",DateTime.Now.Second);
                            Console.WriteLine("当前价格b{0}", tmpOrder.Price);
                            tmpOrder.Price += 20;
                            orderRepository.Update(tmpOrder);
                            // using (var _context = orderRepository.UnitOfWork) 
                            {
                                orderRepository.UnitOfWork.Commit();
                            }

                            Console.WriteLine("当前价格b1 {0}", tmpOrder.Price);
                            //orderRepository.Context.Dispose();
                            Console.WriteLine("当前线程1 {0}", Thread.CurrentThread.ManagedThreadId);
                        });
                        // var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
                        //using (var unitOfWork = LocalServiceLocator.GetService<IEFUnitOfWork>("order") /*LocalServiceLocator.GetService<IDbContextFactory>().CreateContext<YmtTradingUnitOfwork>()*/)
                        //{
                        //    var orderRepository = LocalServiceLocator.GetService<IRepositoryFactory>().CreateRepository<YmtOrder, YmtOrderRepository>(unitOfWork);
                        //    tmpOrder = orderRepository.FindOne("0000b222-db89-4c4b-b226-1fc694d3d065");
                        //    Console.WriteLine("当前价格b {0}", tmpOrder.Price);
                        //    tmpOrder.Price += 20;
                        //    orderRepository.Update(tmpOrder);

                        //    //using (var _context = orderRepository.UnitOfWork)
                        //    //{
                        //    orderRepository.UnitOfWork.Commit();
                        //    //}

                        //    Console.WriteLine("当前价格b1 {0}", tmpOrder.Price);
                        //    Console.WriteLine("当前线程 {0}", Thread.CurrentThread.ManagedThreadId);
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                });
                var th2 = new Task(() =>
                {
                    YmtOrder tmpOrder = null;

                    try
                    {
                        //new YmtOrderRepository(new EFUnitOfWork("orderContext")); //
                        //var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
                        OrderRepo(orderRepository =>
                        {
                            tmpOrder = orderRepository.FindOne("0000b222-db89-4c4b-b226-1fc694d3d065");
                            //tmpOrder.Name = string.Format("并发测试{0}",DateTime.Now.Second);
                            Console.WriteLine("当前价格a {0}", tmpOrder.Price);
                            tmpOrder.Price += 10;
                            orderRepository.Update(tmpOrder);
                            // using (var _context = orderRepository.UnitOfWork) 
                            {
                                orderRepository.UnitOfWork.Commit();
                            }

                            Console.WriteLine("当前价格a2 {0}", tmpOrder.Price);
                            //orderRepository.Context.Dispose();
                            Console.WriteLine("当前线程 {0}", Thread.CurrentThread.ManagedThreadId);
                        });
                        //using (var unitOfWork = LocalServiceLocator.GetService<IEFUnitOfWork>("order") /*LocalServiceLocator.GetService<IDbContextFactory>().CreateContext<YmtTradingUnitOfwork>()*/)
                        //{
                        //    var orderRepository = LocalServiceLocator.GetService<IRepositoryFactory>().CreateRepository<YmtOrder, YmtOrderRepository>(unitOfWork);
                        //    tmpOrder = orderRepository.FindOne("0000b222-db89-4c4b-b226-1fc694d3d065");
                        //    //tmpOrder.Name = string.Format("并发测试{0}",DateTime.Now.Second);
                        //    Console.WriteLine("当前价格a {0}", tmpOrder.Price);
                        //    tmpOrder.Price += 10;
                        //    orderRepository.Update(tmpOrder);
                        //    // using (var _context = orderRepository.UnitOfWork) 
                        //    {
                        //        orderRepository.UnitOfWork.Commit();
                        //    }

                        //    Console.WriteLine("当前价格a2 {0}", tmpOrder.Price);
                        //    //orderRepository.Context.Dispose();
                        //    Console.WriteLine("当前线程 {0}", Thread.CurrentThread.ManagedThreadId);
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                });
                listTask.Add(th);
                listTask.Add(th2);
            }

            listTask.Each(e => e.Start());
            Task.WaitAll(listTask.ToArray());
        }
        public TResult OrderRepo<TResult>(Func<IYmtOrderRepository, TResult> repoAction)
        {
            using (var unitOfWork = LocalServiceLocator.GetService<IDbContextFactory>().CreateContext<YmtTradingUnitOfwork>(e=>new YmtTradingUnitOfwork(e)))
            {
                var orderRepository = LocalServiceLocator.GetService<IRepositoryFactory>().CreateRepository<YmtOrder, YmtOrderRepository>(unitOfWork);
                return repoAction(orderRepository);
            }
        }
        public void OrderRepo(Action<IYmtOrderRepository> repoAction)
        {
            using (var unitOfWork = LocalServiceLocator.GetService<IEFUnitOfWork>("order"))
            {
                var orderRepository = LocalServiceLocator.GetService<IRepositoryFactory>().CreateRepository<YmtOrder, YmtOrderRepository>(unitOfWork);
                repoAction(orderRepository);
            }
        }
        [Test]
        [Description("ef 1对N 关系")]
        public void OneToMany_Order()
        {
            var order = new YmtOrder(Guid.NewGuid().ToString(), "test order", 12M);
            //
            order.AddOrderLine(new OrderLine(100))
            .AddOrderLine(new OrderLine(120))
            .AddOrderLine(new OrderLine(150))
            .SetAddress(new ReceiptAddress { City = "A", Zip = "B", Street = "C" });
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            orderRepository.Add(order);
            var x = orderRepository.Context.AsyncCommit();
            Assert.True(x.Result.ResultData > 0);
        }
        [Test]
        [Description("测试top N")]
        public void Select_top_n()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();

            var order = orderRepository.FindTop<DateTime>(Specifications.Specification<YmtOrder>.CreateAnySpecification().SatisfiedBy()
                , 2, e => e.CreateTime, SortOrder.Descending);
            Assert.AreEqual(2, order.Count());
        }
        [Test]
        [Description("ef 1对1 关系")]
        public void OneToOne_Order()
        {
            var order = new YmtOrder(Guid.NewGuid().ToString("N"), "TT", 55.78M);
            Assert.AreEqual(true, order.Validator().ResultData.Success);
            //1:1
            order.SetAddress(new ReceiptAddress { City = "A", Street = "C", Zip = "D" });
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            orderRepository.Add(order);
            var x = orderRepository.Context.Commit();
            Assert.AreEqual(1, x.ResultData);
        }

        [Test]
        public void SaveBath_Order()
        {

            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            //var orderList = new HashSet<Ymt_OrderLine>();
            for (var i = 0; i < 50000; i++)
            {
                //1.
                var order = new YmtOrder(Guid.NewGuid().ToString(), "ok", 20M);

                order.SetAddress(new ReceiptAddress { City = "A", Street = "B", Zip = "CCC" });
                //2.
                //这里是DTO映射到实体
                //var order = orderDto.MapTo<OrderDto, Ymt_Order>();

                //3.
                //这里是实体验证（实体需要继承 IValidatableObject 接口），主意：如果不显示执行xx.Validatory 方法，程序会自动隐式执行实体验证
                var val = order.Validator();
                Assert.AreEqual(true, val.Success, val.Message);
                //验证的原始写法
                //var entityValidator = EntityValidatorFactory.CreateValidator();
                //Assert.AreEqual(false, entityValidator.IsValid(order));

                //3.
                //
                orderRepository.Add(order);
            }
            //4
            var result = orderRepository.Context.Commit();
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(3, result.ResultData);
        }

        [Test]
        public void FindOne_Order()
        {
            var orderRepository = LocalServiceLocator.GetService<IRepositoryFactory>().CreateRepository<YmtOrder, YmtTradingUnitOfwork, YmtOrderRepository>(e => new YmtOrderRepository(e));// LocalServiceLocator.GetService<IYmtOrderRepository>();

            var order = orderRepository.FindOne("97ec2cd1-650a-422a-af72-1b19c1bbc5d8");
            Assert.AreEqual(null, order);
        }

        [Test]
        public void FindList_Order()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var allList = orderRepository.Find(Specifications.Specification<YmtOrder>.CreateAnySpecification().SatisfiedBy());
            Assert.IsNotNull(allList);
        }

        [Test]
        public void FindPageList_Order()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var allList = orderRepository.Find(Specifications.Specification<YmtOrder>.CreateAnySpecification().SatisfiedBy(), e => e.Id, System.Data.SqlClient.SortOrder.Ascending, 1, 5);
            Assert.AreEqual(5, allList.Data.Count());

        }

        [Test]
        public void SaveOne_User()
        {
            var user = new YmtUser(Guid.NewGuid().ToString(), "lg");

            var userVali = user.Validator();
            Assert.AreEqual(true, userVali.Success, userVali.Message);
            LocalServiceLocator.GetService<IYmtUserRepository>();
            var userRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            userRepository.Add(user);
            userRepository.Context.Commit();
        }
        [Test]
        public void ModifyOne_User()
        {
            var userRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var user = userRepository.FindOne("1fd34e9e-bf81-4328-85f0-bb8e2bf71c20");
            user.UName = "testaaaa";
            userRepository.Update(user);

            var submit = userRepository.Context.Commit();
            user = userRepository.FindOne("1fd34e9e-bf81-4328-85f0-bb8e2bf71c20");
            user.UName = "t";

            userRepository.Update(user);
            userRepository.Context.Commit();
            Assert.AreEqual(1, submit.ResultData);
        }
        [Test]
        public void RollbackModifyOne_User()
        {
            var userRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var user = userRepository.FindOne("de01ec83-11b4-412e-af38-60d7acc490d4");
            if (user == null)
            {
                Console.WriteLine("用户不存在");
                return;
            }
            user.UName = "testaaaa";
            userRepository.Update(user);
            var submit = userRepository.Context.Commit();
            user.UName = "ss";
            userRepository.Update(user);
            userRepository.Context.Commit();
        }
        [Test]
        public void RemoveOne_User()
        {
            var userRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var user = userRepository.FindOne("5ae16974-190a-4102-b887-069802cc19a6");
            if (user != null)
            {
                userRepository.Remove(user);
                var result = userRepository.Context.Commit();
                Console.WriteLine(result.ResultData);
            }
            else
            {
                Console.WriteLine("用户不存在");
            }
        }
        [Test]
        public void FindAll_User()
        {
            var userRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var allUser = userRepository.FindAll();
            Assert.NotNull(allUser);
        }
        //显示加载指定的关联对象_
        //这会产生 left join
        [Test]
        public void Find_Include_Order()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            var result = orderRepository.Find(Specifications.AnySpecification<YmtOrder>.CreateAnySpecification().SatisfiedBy()
                , e => e.OrderLineList
                , e => e.RAddress);
            //Assert.AreEqual(100006, result.Count());

        }
        //查找指定的对象_规约模式
        [Test]
        [Description("查找指定的对象_规约模式")]
        public void Find_Specification_User()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var result = orderRepository.Find(Specifications.Specification<YmtUser>.Parse(e => !e.IsDelete).SatisfiedBy());
            Assert.AreEqual(result.Count(), result.Count());
        }
        //查找所有元素并按指定的元素排序
        [Test]
        [Description("查找所有元素并按指定的元素排序")]
        public void Find_Specification_Sort_User()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var result = orderRepository.Find(e => e.ModifyTime, System.Data.SqlClient.SortOrder.Descending);
            Assert.AreEqual(result.First().Id, result.First().Id);
        }
        [Test]
        [Description("查找指定的元素按指定的字段排序")]
        public void Find_S_S_D_User()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var result = orderRepository.Find(Specifications.Specification<YmtUser>.Parse(e => e.IsDelete).SatisfiedBy(), e => e.ModifyTime, System.Data.SqlClient.SortOrder.Descending);
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        [Description("查询翻页")]
        public void Find_Pages_User()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var result = orderRepository.Find(Specifications.Specification<YmtUser>.Parse(e => !e.IsDelete).SatisfiedBy(), e => e.ModifyTime, System.Data.SqlClient.SortOrder.Descending, 1, 3);
            Assert.AreEqual(3, result.PageSize);
            Assert.AreEqual(1, result.PageNumber);
            //Assert.AreEqual(11, result.TotalRecords);
            //Assert.AreEqual(4, result.TotalPages);
        }
        [Test]
        [Description("执行脚本")]
        public void Find_ExecuteScript()
        {
            var orderRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var result = orderRepository.ExecuteQuery("select * from [ymt_trading_user_3] with(nolock)");
            Assert.NotNull(result);
            //Assert.AreEqual("2cfa3f9c-dd6d-4eb0-93f3-1750dd726e02", result.First().Id);
        }
        [Test]
        public void RemoveMultiple_User()
        {
            var userRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var user = userRepository.FindOne("6e9a1c5a-4f2d-4824-a0cc-1733b8498dc8");
            var user2 = userRepository.FindOne("72a28d9b-8195-4f6f-8cad-8900b163ac0a");

            userRepository.Remove(user, user2);
            var result = userRepository.Context.Commit();
            Console.WriteLine(result.ResultData);
        }
        [Test]
        public async void AsyncFindOne_User()
        {
            var userRepository = LocalServiceLocator.GetService<IYmtUserRepository>();
            var user = await userRepository.FindOneAsync(TimeSpan.FromSeconds(0.5), "1fd34e9e-bf81-4328-85f0-bb8e2bf71c20");

            Console.WriteLine("async end");

            Assert.AreEqual("t", user.UName);
        }
        [Test]
        [Description("手动执行表脚本后DBContext改变")]
        public void NewTable_T_Entity()
        {
            var t = new T1(Guid.NewGuid().ToString("N"), 131, DateTime.Now, "AAA");
            var validatorResult = t.Validator();
            Assert.AreEqual(true, validatorResult.ResultData.Success);
            var tRepository = LocalServiceLocator.GetService<ITRepository>();
            tRepository.Add(t);
            var result = tRepository.Context.Commit();
            Assert.AreEqual(1, result.ResultData);
        }
        [Test]
        [Description("并发修改")]
        public void ConCurrent_Update()
        {
            var tRepository = LocalServiceLocator.GetService<ITRepository>();
            var t = tRepository.FindOne("0001f39c16e14dd5a88d13187363932b");
            t.IsDelete = true;
            Action a = () =>
            {
                tRepository.Update(t);
                tRepository.Context.Commit();
            };
            Action b = () =>
            {    //这里更新失败
                t.IsDelete = false;
                tRepository.Update(t);
                tRepository.Context.Commit();
            };

            Parallel.Invoke(a, b);
        }
        [Test]
        [Description("手动执行表脚本后DBContext改变")]
        public void AddOrUpdate_T_Entity()
        {
            var t = new T1("fb2d55a921734029aa72521255efbc63", 100, DateTime.Now, "AAA");
            var validatorResult = t.Validator();
            Assert.AreEqual(true, validatorResult.ResultData.Success);
            var tRepository = LocalServiceLocator.GetService<ITRepository>();
            tRepository.AddOrUpdate(e => e.Id, t);
            //  tRepository.Add(t);
            //tRepository.Update(t);
            var result = tRepository.Context.Commit();
            Assert.AreEqual(1, result.ResultData);
        }
    }
}
