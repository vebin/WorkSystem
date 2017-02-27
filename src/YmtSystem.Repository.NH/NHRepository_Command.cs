using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace YmtSystem.Repository.NH
{
    public partial class NHRepository<TEntity> where TEntity : class
    {
        public virtual void Add(TEntity entity)
        {
            this.UnitOfWork.RegisterNew(entity);
        }

        public virtual void Update(TEntity entity)
        {
            this.UnitOfWork.RegisterModified(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            this.UnitOfWork.RegisterDeleted(entity);
        }
    }
}
