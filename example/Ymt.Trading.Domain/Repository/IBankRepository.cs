using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.Account;
using YmtSystem.Domain.Repository;

namespace Ymt.Trading.Domain.Repository
{
    public interface IBankRepository : IRepository<Bank>
    {
    }
}
