using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace Ymt.Trading.Domain.Model.Table
{
    public class T8 : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete
    {
        public bool IsDelete { get; set; }
        public byte[] Version { get; set; }
        public int T81 { get; set; }
        public string T82 { get; set; }
        public T8(string id) 
        {
            this.Id = id;

        }
        protected T8() 
        { }
    }
}
