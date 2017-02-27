

namespace Ymt.Trading.Domain.Model.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;

    public class Bank : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete
    {
        public bool IsDelete { get; set; }
        
        public byte[] Version { get; set; }
        private List<UserAccount> accountList = new List<UserAccount>();
        public string BankName { get; set; }
        public BankAddress Address { get; set; }

        public Bank()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public Bank AddAccount(UserAccount ua)
        {
            accountList.Add(ua);
            return this;
        }
        public Bank AddAddress(BankAddress ba)
        {
            this.Address = ba;
            return this;
        }
        public List<UserAccount> UserAccount { get; set; }
    }
}
