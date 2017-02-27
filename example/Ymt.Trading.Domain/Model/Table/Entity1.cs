using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;
namespace Ymt.Trading.Domain.Model.Table
{
    public class Entity1 : AggregateRoot<string>, IConcurrencyCheck, IEntityLogicDelete
    {
        public bool IsDelete { get; set; }
        public byte[] Version { get; set; }
        private string _id;
        public override string Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                this._id = value;
            }
        }
        public DateTime DT { get; set; }
        public Entity1(string id) 
        {
            this.Id = id;
        }
        protected Entity1() { }
    }


    public class Entity2 : Entity
    {
        public string Name { get; set; }
        public int EId { get; set; }

        protected override IEnumerable<object> CompareComponents()
        {
            yield return Name;
            yield return EId;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext);
        }
        public Entity2(string id) 
        {
            this.Id = id;
        }
        protected Entity2() 
        { }
    }
}
