using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace Ymt.Trading.Domain.Model.Table
{
    public class T7 : ValueObject<T7>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public T5 T5Field { get; set; }
    }


}
