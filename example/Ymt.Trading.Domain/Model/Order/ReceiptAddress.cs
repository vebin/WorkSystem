using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;
namespace Ymt.Trading.Domain.Model.Order
{
    public class ReceiptAddress : ValueObject<ReceiptAddress>
    {
      
        /// <summary>
        /// 获取或设置“地址”类型中“市”部分的信息。
        /// </summary>
        public string City
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置“地址”类型中“街道”部分的信息。
        /// </summary>
        public string Street
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置“地址”类型中“邮政区码”部分的信息。
        /// </summary>
        public string Zip
        {
            get;
            set;
        }
        
        protected override IEnumerable<object> EqualityComponents()
        {
            yield return this.City;
            yield return this.Zip;
        }
    }
}
