using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.Table;
using YmtSystem.Domain.Repository;
using YmtSystem.Repository.EF;

namespace Ymt.Trading.Domain.Repository
{
    public class T1Repository : EFRepository<T1>, ITRepository
    {
        public T1Repository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
