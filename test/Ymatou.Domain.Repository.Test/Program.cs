using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.Order;
using YmtSystem.CrossCutting;
using YmtSystem.Domain.Shard.Validator;
using YmtSystem.Domain.Shard;
using System.Diagnostics;
using YmtSystem.Repository.EF.BulkProvider;
namespace YmtSystem.Domain.Repository.Test
{
    class Program
    {
        public static void SetUp()
        {
            YmatouFramework.Start();
        }

        public static void Stop()
        {
            YmatouFramework.Stop();
        }
        static void Main(string[] args)
        {
            var list = new List<YmtOrder>(50000);
            for (var i = 0; i < 3; i++)
            {
                //1.
                var order = YmtOrder.Create("ok", 20M);
                order.SetAddress(new ReceiptAddress { City = "A", Street = "B", Zip = "CCC" });
                //2.
                //这里是DTO映射到实体
                //var order = orderDto.MapTo<OrderDto, Ymt_Order>();

                //3.
                //这里是实体验证（实体需要继承 IValidatableObject 接口），主意：如果不显示执行xx.Validatory 方法，程序会自动隐式执行实体验证
                var val = order.Validator();
                if (!val.Success)
                    throw new Exception("出错");
                //验证的原始写法
                //var entityValidator = EntityValidatorFactory.CreateValidator();
                //Assert.AreEqual(false, entityValidator.IsValid(order));

                //3.
                //
                // orderRepository.Add(order);
                list.Add(order);
            }

            Console.Read();
            SetUp();
            var orderRepository = LocalServiceLocator.GetService<IYmtOrderRepository>();
            //var orderList = new HashSet<Ymt_OrderLine>();
            var watch = Stopwatch.StartNew();

            for (var i = 0; i < 50000; i++)
            {
                //1.
                var order = YmtOrder.Create("ok", 20M);
                order.SetAddress(new ReceiptAddress { City = "A", Street = "B", Zip = "CCC" });
                //2.
                //这里是DTO映射到实体
                //var order = orderDto.MapTo<OrderDto, Ymt_Order>();

                //3.
                //这里是实体验证（实体需要继承 IValidatableObject 接口），主意：如果不显示执行xx.Validatory 方法，程序会自动隐式执行实体验证
                var val = order.Validator();
                if (!val.Success)
                    throw new Exception("出错");
                //验证的原始写法
                //var entityValidator = EntityValidatorFactory.CreateValidator();
                //Assert.AreEqual(false, entityValidator.IsValid(order));

                //3.
                //
                // orderRepository.Add(order);
                list.Add(order);
            }
            Console.WriteLine("添加耗时1 {0} 秒", watch.ElapsedMilliseconds / 1000.0);
            orderRepository.Add(list);
            Console.WriteLine("添加耗时2 {0} 秒", watch.ElapsedMilliseconds / 1000.0);
            //4
            var result = orderRepository.Context.Commit();
            Console.WriteLine("结果：{0},写入耗时 {1} 秒", result.ResultData, watch.ElapsedMilliseconds / 1000.0);
            Stop();
            Console.Read();
        }
    }
}
