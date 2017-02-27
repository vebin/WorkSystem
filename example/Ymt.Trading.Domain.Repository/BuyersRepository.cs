using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.User;
using YmtSystem.Domain.Repository;
using YmtSystem.Repository.EF;

namespace Ymt.Trading.Domain.Repository
{
    public class BuyersRepository : EFRepository<Buyers>, IBuyersRepository
    {
        public BuyersRepository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
