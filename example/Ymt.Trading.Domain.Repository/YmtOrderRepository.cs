using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Repository;
using YmtSystem.Repository.EF;
using Ymt.Trading.Domain.Model.Order;

namespace Ymt.Trading.Domain.Repository
{
    public class YmtOrderRepository : EFRepository<YmtOrder>, IYmtOrderRepository
    {
        public YmtOrderRepository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
