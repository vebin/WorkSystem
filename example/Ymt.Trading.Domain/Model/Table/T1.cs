using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace Ymt.Trading.Domain.Model.Table
{
    public class T1 : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete,IEntityExtend
    {
        public bool IsDelete { get; set; }
        public int T1Key { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public int A { get; set; }
        public DateTime B { get; set; }
        public string T { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (A < 10) yield return new ValidationResult(" ErrorMessage = A<10?");
        }
        public T1(string id,int a,DateTime b,string t) 
        {
            this.CreateTime = DateTime.Now;
            this.ModifyTime = DateTime.Now;
            this.IsDelete = false;
            this.Id = id;
            this.A = a;
            this.B = b;
            this.T = t;

        }
        protected T1() 
        { }
    }
}
