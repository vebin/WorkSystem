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
    public class T11Repository : EFRepository<T11>, IT11Repository
    {
        public T11Repository(IUnitOfWork context)
            : base(context)
        {
        }
    }
}
