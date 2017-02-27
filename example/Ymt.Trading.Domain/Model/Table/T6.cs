using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace Ymt.Trading.Domain.Model.Table
{
    public class T6 : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete
    {
        public bool IsDelete { get; set; }
        public byte[] Version { get; set; }
        public int C { get; set; }
        public int D { get; set; }

    }
}
