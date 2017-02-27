using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Repository.NH.Context;
using YmtSystem.Repository.NH.Test.Mapping;

namespace YmtSystem.Repository.NH.Test.Context
{
    /// <summary>
    /// 订单会话上下文
    /// </summary>
    public class OrderSessionContext : DbSessionContext
    {
        public OrderSessionContext(string nhXmlPath)
            : base(null, nhXmlPath,false,false)
        {

        }

        protected override void OnEntityMap(EntityClassMap map)
        {
            map.AddMap(new OrderMapping());
        }
    }

    /// <summary>
    /// 用户会话上下文
    /// </summary>
    public class UserSessionContext : DbSessionContext
    {
        public UserSessionContext(string nhXmlPath)
            : base(null, nhXmlPath,true,false)
        {
          
        }
      
        protected override void OnEntityMap(EntityClassMap map)
        {
            map.AddMap(new UserMapping());
        }
    }
}
