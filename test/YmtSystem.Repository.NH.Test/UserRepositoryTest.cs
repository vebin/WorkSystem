using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using YmtSystem.Repository.NH.Test.Context;
using YmtSystem.Repository.NH.Test.Repository;

namespace YmtSystem.Repository.NH.Test
{
    [TestFixture]
    public class UserRepositoryTest
    {
        [Test]
        public void SaveUser()
        {
            using (var unitOfWork = new UserUnitOfWork())
            {
                var userRepository = new UserRepository(unitOfWork);
                userRepository.Add(new Domain.User { Id = Guid.NewGuid().ToString(), BuyerName = "test1", dMoney = 12.0M, sName = "ok" });
                userRepository.Add(new Domain.User { Id = Guid.NewGuid().ToString(), BuyerName = "test1", dMoney = 12.0M, sName = "ok" });
                userRepository.Add(new Domain.User { Id = Guid.NewGuid().ToString(), BuyerName = "test1", dMoney = 12.0M, sName = "ok" });
                userRepository.UnitOfWork.Commit();
            }
        }
    }
}
