namespace YmtSystem.Repository.EF.Factory
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DbContextFactory 
    /// </summary>
    public interface IDbContextFactory
    {
        TContext CreateContext<TContext>(Func<string, TContext> factory) where TContext : YmtSystemDbContext;
        TContext CreateContext<TContext>() where TContext : YmtSystemDbContext;
        YmtSystemDbContext CreateContext(Type storageContextType);
    }
}
