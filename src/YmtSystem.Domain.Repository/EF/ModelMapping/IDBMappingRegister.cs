namespace YmtSystem.Domain.Repository.EF.ModelMapping
{
    using System.Data.Entity.ModelConfiguration.Configuration;

    public interface IDBMappingRegister
    {
        void Register(ConfigurationRegistrar cfg);
    }
}
