using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;
 using System.ComponentModel.DataAnnotations ;
namespace Ymt.Trading.Domain.Model.Table
{
    public class T2 : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete
    {
        public bool IsDelete { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public int T1 { get; set; }
        public int T3 { get; set; }
        public int T4 { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public T2(string id,int t) 
        {
            this.Id =id ;
            this.T1 = t;
            this.CreateTime = DateTime.Now;
        }
        protected T2() { }
    }
}
