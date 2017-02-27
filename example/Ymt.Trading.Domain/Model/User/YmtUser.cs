using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace Ymt.Trading.Domain.Model.User
{
    public class YmtUser : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete,IEntityExtend
    {
        public bool IsDelete { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空")]
        public string UName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
        public YmtUser(string id, string name)
        {
            this.Id = id;
            this.UName = name;
            this.CreateTime = DateTime.Now;
        }
        protected YmtUser() { }
    }
}
