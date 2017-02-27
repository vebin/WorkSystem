using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmtSystem.Repository.NH.Test.Domain
{
    public class Order
    {
        public virtual string Id { get; set; }
        public virtual string sName { get; set; }
        public virtual decimal dMoney { get; set; }
        public virtual string BuyerName { get; set; }

    }
}
