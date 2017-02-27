using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Model.Table;
using YmtSystem.Repository.EF.ModelMapping;

namespace Ymt.Trading.Domain.Repository.Mapping
{
    public class T2Mapping : EntityTypeConfiguration<T2>, IDBMappingRegister
    {
        /// <summary>
        /// 一个类映射到多个表—实体拆分
        /// 字表T_3 自动生成外键Id(依赖T_2表)，如果库不存在，则会自动创建表结构，否则需要手动执行脚本;
	    /// 另外，不要遗漏任何字段，否则ef 会自动帮我们创建表，保存这些遗漏的字段
        /// </summary>
        public T2Mapping()
        {
            Map(e =>
            {
                e.ToTable("T_2");
                // e.MapInheritedProperties();
                HasKey(_e => _e.Id);            
                e.Properties<int>(_e => _e.T1);
                e.Properties<int>(_e => _e.T3);
                e.Properties<int>(_e => _e.T4);             
            })
            .Map(e =>
            {
                e.ToTable("T_3");
                //e.MapInheritedProperties();

                e.Properties<DateTime>(_e => _e.CreateTime);
                e.Properties<DateTime?>(_e => _e.ModifyTime);
               // e.Properties<byte[]>(_e => _e.Version);
                e.Properties<bool>(_e => _e.IsDelete);
            });            
        }

        public void Register(ConfigurationRegistrar cfg)
        {
            cfg.Add(this);
        }
    }
}
