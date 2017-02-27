namespace YmtSystem.Repository.EF.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting;

    internal sealed class YmtSystemDbContextScope<TEntity> where TEntity : class
    {
        public static TEntity GetEntity(Func<TEntity> valFactory, DbContextLifeScope scope)
        {
            if (scope == DbContextLifeScope.New) return new NewScope().Get(valFactory);
            if (scope == DbContextLifeScope.SameThread) return  ThreadLocalScope.Get(valFactory);
            if (scope == DbContextLifeScope.SameHttpContext) return new HttpContextScope().Get(valFactory);
            throw new Exception<EFRepositoryException>("错误的DbContextScope");
        }
        public static void TryClear()
        {
            //ThreadLocalScope.TryCleaar();
            HttpContextScope.TryCleaar();
            NewScope.TryCleaar();
        }
        private class ThreadLocalScope
        {
            private static readonly ThreadLocal<TEntity> pool = new ThreadLocal<TEntity>();

            public static TEntity Get(Func<TEntity> valFactory)
            {
                if (pool.Value == null || pool.Value == default(TEntity))
                {
                    TEntity tmpEntity = default(TEntity);
                    System.Threading.Interlocked.CompareExchange<TEntity>(ref tmpEntity, valFactory(), default(TEntity));
                    pool.Value = tmpEntity;
                }

                return pool.Value;
            }
            public void TryCleaar()
            {

            }
        }

        private class HttpContextScope
        {
            private HttpContextLifetimeManager<TEntity> pool = new HttpContextLifetimeManager<TEntity>();

            public TEntity Get(Func<TEntity> valFactory)
            {
                var obj = pool.GetValue() as TEntity;
                if (obj == default(TEntity))
                {
                    TEntity tmpEntity = default(TEntity);
                    System.Threading.Interlocked.CompareExchange<TEntity>(ref tmpEntity, valFactory(), default(TEntity));
                    pool.SetValue(tmpEntity);
                }
                return obj;
            }
            public static void TryCleaar()
            {
            }
        }

        private class NewScope
        {
            public TEntity Get(Func<TEntity> valFactory)
            {
                TEntity tmpEntity = default(TEntity);
                System.Threading.Interlocked.Exchange(ref tmpEntity, valFactory());
                return tmpEntity;
            }
            public static void TryCleaar()
            {
            }
        }
    }
}
