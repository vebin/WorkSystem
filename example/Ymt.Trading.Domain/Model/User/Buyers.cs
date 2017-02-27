using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace Ymt.Trading.Domain.Model.User
{
    public class Buyers : YmtUser //AggregateRoot, IValidatableObject
    {
       
        public int Star { get; set; }
        public string Tel { get; set; }
        public byte Sex { get; set; }
        public byte Sex2 { get; set; }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return null;
        }
        public Buyers(string id) 
        {
            this.Id = id;
        }
        protected Buyers() { }

    }
}
