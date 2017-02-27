namespace YmtSystem.Repository.EF.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Contexts;
    using YmtSystem.Repository.EF.DBAttribute;

    public class DbContextFactory : IDbContextFactory
    {
        public DbContextFactory()
        {
        }
        public TContext CreateContext<TContext>(Func<string,TContext> factory) where TContext : YmtSystemDbContext
        {
            var contextType = typeof(TContext);
           
            var contextAttribute = contextType.GetCustomAttributes(false).OfType<StoreContextAttribute>().SingleOrDefault();
            if (contextAttribute == null)
                contextAttribute = new StoreContextAttribute();

            var contextName = contextAttribute.StoreContextName;
            var scope = DbConfigure.GetConfigure(contextName).Scope;
            return YmtSystemDbContextScope<TContext>.GetEntity(() => factory(contextName), scope);
        }
        public TContext CreateContext<TContext>() where TContext : YmtSystemDbContext
        {
            return (TContext)CreateContext(typeof(TContext));
        }
        public YmtSystemDbContext CreateContext(Type storageContextType)
        {
            var contextType = storageContextType;
            var contextAttribute = contextType.GetCustomAttributes(false).OfType<StoreContextAttribute>().SingleOrDefault();
            if (contextAttribute == null)
                contextAttribute = new StoreContextAttribute();

            var contextName = contextAttribute.StoreContextName;
            var scope = DbConfigure.GetConfigure(contextName).Scope;
            return YmtSystemDbContextScope<YmtSystemDbContext>.GetEntity(() => (YmtSystemDbContext)Activator.CreateInstance(contextType, contextName), scope);
        }
    }
}
