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
    public class T5Repository : EFRepository<T5>, IT5Repository
    {
        public T5Repository(IUnitOfWork context)
            : base(context)
        {
        }
    }

    public class T6Repository : EFRepository<T6>, IT6Repository
    {
        public T6Repository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
