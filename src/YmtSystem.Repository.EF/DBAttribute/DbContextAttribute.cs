namespace YmtSystem.Repository.EF.DBAttribute
{
    using System;
    using YmtSystem.Repository.EF.Factory;

    /// <summary>
    /// 存储上下文
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class StoreContextAttribute : Attribute
    {
        public string StoreContextName { get; private set; }
        public string DbFileName { get; private set; }
        public StoreContextAttribute()
            : this(null, null)
        {
        }

        public StoreContextAttribute(string contextName, string dbFileName="db.cfg")
        {
            this.StoreContextName = contextName ?? "DefaultContext";
            this.DbFileName = dbFileName;
        }
    }
}
