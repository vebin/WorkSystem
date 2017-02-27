using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Repository.NH.ModelMapping;
using YmtSystem.Repository.NH.Test.Domain;

namespace YmtSystem.Repository.NH.Test.Mapping
{
    public class UserMapping : ModelMappingBase<User>
    {
        public UserMapping()
        {
            Table("usertest");
            Id(e => e.Id);

            Map(e => e.BuyerName).Column("sBuyerName");
            Map(e => e.sName).Column("sName");
            Map(e => e.dMoney).Column("dMoney");
            Map(e => e.CreateTime).Column("dCDate").Nullable();
        }
    }
}
