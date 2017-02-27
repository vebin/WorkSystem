

namespace Ymt.Trading.Domain.Model.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;

    public class UserAccount : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete,IEntityExtend
    {
        public bool IsDelete { get; set; }
       
        public byte[] Version { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public UserAccount()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
            this.Money = 0.0M;
        }
        public String AName { get; set; }
       
        public decimal Money { get; set; }
        public Bank UserAccountBankInfo { get; set; }


        public override IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
