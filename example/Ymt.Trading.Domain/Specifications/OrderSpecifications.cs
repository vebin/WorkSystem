using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.Order;
using YmtSystem.Domain.Specifications;
namespace Ymt.Trading.Domain.Specifications
{
    public class OrderSpecifications
    {
        public static ISpecification<YmtOrder> Find()
        {
            //TODO:这里完成“规格”
            return Specification<YmtOrder>.Parse(e => e.Id != "");
        }
    }
}
