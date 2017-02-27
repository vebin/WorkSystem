using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmtSystem.Repository.NH.Test.Context
{
    public class OrderUnitOfWork : NHUnitOfWork
    {
        public OrderUnitOfWork()
            : base(new OrderSessionContext(null))
        {

        }
    }

    public class UserUnitOfWork : NHUnitOfWork
    {
        public UserUnitOfWork()
            : base(new UserSessionContext(null))
        {

        }
    }
}
