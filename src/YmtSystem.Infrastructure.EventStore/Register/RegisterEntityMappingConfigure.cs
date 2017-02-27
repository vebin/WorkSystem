using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace YmtSystem.Infrastructure.EventStore.Repository.Register
{
    public abstract class RegisterEntityMappingConfigure
    {
        private static Dictionary<Type, EntityMappingConfigure> mappingMeatadate = new Dictionary<Type, EntityMappingConfigure>();

        public abstract EntityMappingConfigure RegisterEntity();

        public void Execute()
        {
            var cfg = RegisterEntity();
            mappingMeatadate[cfg.MappType] = cfg;
        }

        public static Dictionary<Type, EntityMappingConfigure> MetaData { get { return mappingMeatadate; } }
    }

    public class EntityMappingSource
    {
        public static EntityMappingConfigure GetMapping(Type type, bool notfindThrowOut = true)
        {
         
            EntityMappingConfigure cfg;
            if (RegisterEntityMappingConfigure.MetaData.TryGetValue(type, out cfg))
                return cfg;
            else throw new KeyNotFoundException(string.Format("未找到 {0}", type.Name));
        }
    }

    public class EntityMappingConfigure
    {
        public Type MappType { get; set; }
        public string ToDatabase { get; set; }
        public string ToCollection { get; set; }
        public List<IndexConfigure> IndexCfg { get; set; }
    }

    public class IndexConfigure
    {
        public string IndexName { get; set; }
        public bool AscOrder { get; set; }
    }
}
