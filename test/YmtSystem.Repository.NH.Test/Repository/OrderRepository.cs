using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Repository.NH.Test.Domain;

namespace YmtSystem.Repository.NH.Test.Repository
{
    public class OrderRepository : NHRepository<Order>
    {
        public OrderRepository(INHUnitOfWork unitofWork)
            : base(unitofWork)
        {
         
        }
    }


    public class UserRepository : NHRepository<User>
    {
        public UserRepository(INHUnitOfWork unitofWork)
            : base(unitofWork)
        {

        }
    }
}
