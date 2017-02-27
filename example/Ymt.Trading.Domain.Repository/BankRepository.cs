using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.Account;
using YmtSystem.Domain.Repository;
using YmtSystem.Repository.EF;

namespace Ymt.Trading.Domain.Repository
{
    public class BankRepository : EFRepository<Bank>, IBankRepository
    {
        public BankRepository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
