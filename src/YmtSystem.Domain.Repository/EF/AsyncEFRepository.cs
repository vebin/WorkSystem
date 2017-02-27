namespace YmtSystem.Domain.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;

    public partial class EFRepository<TAggregateRoot> : Repository<TAggregateRoot>
         where TAggregateRoot : class, IAggregateRoot
    {
        public override async Task<TAggregateRoot> AsyncFindOne(TimeSpan timeOut, params object[] key)
        {
            return await this.EFContext.Context.Set<TAggregateRoot>().FindAsync(key);
        }

        public override Task<IEnumerable<TAggregateRoot>> AsyncFindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }      
    }
}
