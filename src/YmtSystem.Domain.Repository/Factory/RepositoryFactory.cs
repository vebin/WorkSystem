namespace YmtSystem.Domain.Repository.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Repository.EF;

    public class RepositoryFactory
    {
        public static Func<IRepositoryContext> Ef = () =>
         {
             return new EFRepositoryContext();
         };
    }
}
