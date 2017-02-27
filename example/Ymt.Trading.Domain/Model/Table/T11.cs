using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace Ymt.Trading.Domain.Model.Table
{
    public class T11 : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete
    {
        public bool IsDelete { get; set; }
        public byte[] Version { get; set; }
        public int T111 { get; set; }
        public string T112 { get; set; }

        public T11(string id) 
        {
            this.Id = id;
        }
        protected T11() { }
    }
}
