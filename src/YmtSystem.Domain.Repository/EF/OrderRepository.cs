namespace Ymatou.Domain.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ymatou.Domain.Model.Core.Order;
    using Ymatou.Domain.Repository;

    public class OrderRepository : EFRepository<Ymt_Order>, IOrderRepository
    {
        public OrderRepository(IRepositoryContext context) : base(context) 
        {
        }                                        
    }
}
