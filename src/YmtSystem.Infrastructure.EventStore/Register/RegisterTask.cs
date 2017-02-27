namespace YmtSystem.Infrastructure.EventStore.Repository.Register
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting.Utility;
    using YmtSystem.CrossCutting;

    public class RegisterMetaDataTask
    {
        public static void ExecuteTask()
        {
            BuildManagerWrapper
               .Current
               .PublicTypes
               .AsParallel()
               .Where(e =>
                   e != null
                   && !e.FullName.Contains("RegisterEntityMappingConfigure")
                   && e.IsSubclassOf(typeof(RegisterEntityMappingConfigure)))
                .Each(e =>
                    {
                        var obj = Activator.CreateInstance(e);                        
                        e.GetMethod("Execute").Invoke(obj, null);
                       // ((RegisterEntityMappingConfigure)Activator.CreateInstance(e)).Execute(); 
                    });

        }
    }
}
