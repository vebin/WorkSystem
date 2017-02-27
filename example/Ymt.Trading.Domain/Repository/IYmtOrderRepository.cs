namespace Ymt.Trading.Domain.Model.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Repository;

    public interface IYmtOrderRepository : IRepository<YmtOrder>
    {

    }
}
