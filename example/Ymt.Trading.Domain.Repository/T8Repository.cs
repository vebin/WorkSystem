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
    class T8Repository : EFRepository<T8>, IT8Repository
    {
        public T8Repository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
