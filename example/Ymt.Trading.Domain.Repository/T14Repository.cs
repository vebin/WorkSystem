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
    class T14Repository: EFRepository<T14>, IT14Repository
    {
        public T14Repository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
