using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.Repository.NH.Context;
using YmtSystem.Repository.NH.ModelMapping;
using YmtSystem.Repository.NH.Test.Mapping;
using YmtSystem.Repository.NH;
using YmtSystem.Repository.NH.Test.Repository;
using YmtSystem.Repository.NH.Test.Context;

namespace YmtSystem.Repository.NH.Test
{
    [TestFixture]
    public class OrderRepositoryTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
        }
        [Test]
        public void SaveOrder()
        {
            using (var unitOfWork = new OrderUnitOfWork())
            {
                var orderRepository = new OrderRepository(unitOfWork);
                orderRepository.Add(new Domain.Order { Id = Guid.NewGuid().ToString(), BuyerName = "test1", dMoney = 12.0M, sName = "ok" });
                orderRepository.Add(new Domain.Order { Id = Guid.NewGuid().ToString(), BuyerName = "test1", dMoney = 12.0M, sName = "ok" });
                orderRepository.Add(new Domain.Order { Id = Guid.NewGuid().ToString(), BuyerName = "test1", dMoney = 12.0M, sName = "ok" });
                orderRepository.UnitOfWork.Commit();
            }
        }

        [Test]
        public void UpdateOrder()
        {
            using (var unitOfWork = new OrderUnitOfWork())
            {
                var orderRepository = new OrderRepository(unitOfWork);
                orderRepository.Add(new Domain.Order { Id = Guid.NewGuid().ToString(), BuyerName = "test1", dMoney = 12.0M, sName = "ok" });
                orderRepository.Update(new Domain.Order { Id = "a4c30bdf-0fbf-4892-99cd-3475cf16daad", BuyerName = "test3", dMoney = 1000.0M, sName = "ok" });
                orderRepository.Add(new Domain.Order { Id = Guid.NewGuid().ToString(), BuyerName = "test1", dMoney = 12.0M, sName = "ok" });
                orderRepository.UnitOfWork.Commit();
            }
        }

        [Test]
        public void FindOne()
        {
            using (var unitOfWork = new OrderUnitOfWork())
            {
                var orderRepository = new OrderRepository(unitOfWork);
                var order = orderRepository.FindSingleOrDefault(e => e.Id == "a4c30bdf-0fbf-4892-99cd-3475cf16daad");

                Console.WriteLine(order.sName);
            }
        }


        [Test]
        public void FindList()
        {
            using (var unitOfWork = new OrderUnitOfWork())
            {
                var orderRepository = new OrderRepository(unitOfWork);
                var order = orderRepository.Find(e => e.dMoney >= 12.0M);

                Console.WriteLine(order.Count());
            }
        }
        [Test]
        public void FindPageList()
        {
            using (var unitOfWork = new OrderUnitOfWork())
            {
                var orderRepository = new OrderRepository(unitOfWork);

                var result = orderRepository.FindPagedResult(e => e.dMoney >= 12.0M, e => e.BuyerName, System.Data.SqlClient.SortOrder.Ascending, 0, 10);
                Console.WriteLine(result.Data.Count());
                Console.WriteLine(result.PageNumber);
                Console.WriteLine(result.TotalPages);
                Console.WriteLine(result.TotalRecords);
            }
        }
    }
}
