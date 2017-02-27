using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;
using System.ComponentModel.DataAnnotations ;
namespace Ymt.Trading.Domain.Model.Table
{
    public class T5 : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete
    {
        public bool IsDelete { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public T7 T7Field { get; set; }

        public T5(string id) 
        {
            this.Id = id;
        }
        protected T5() { }
    }
  

}
