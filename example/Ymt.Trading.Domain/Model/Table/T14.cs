using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace Ymt.Trading.Domain.Model.Table
{
    public class T14 : AggregateRoot//, IEntityLogicDelete
    {
        //public bool IsDelete { get; set; }
        public string Name { get; set; }
        public int TId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public T14(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.CreateTime = DateTime.Now;
        }

        protected T14() 
        {
        }
    }
}
