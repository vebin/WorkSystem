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
    //public class T5Mapping : EntityTypeConfiguration<T5>, IDBMappingRegister
    //{
    //    /// <summary>
    //    /// 多个实体映射到一个表（表拆分）
    //    /// </summary>
    //    public T5Mapping()
    //    {
    //        //主键
    //        //Map<T5>(e =>
    //        //{
    //            ToTable("T5");
    //            HasKey(_e => _e.Id);
    //            HasRequired(_e => _e.T7Field)
    //                                      .WithRequiredPrincipal(_e => _e.T5Field)
    //                                      .Map(_ => _.MapKey("T7Field"));
    //        //}); ;
    //        //.Map<T5>(e =>
    //        //{
    //        //    e.ToTable("T5");
    //        //    HasKey(_e => _e.Id);
    //        //    //e.Requires(_e => _e.T5Field);
    //        //});

    //    }
    //    public void Register(ConfigurationRegistrar cfg)
    //    {

    //        cfg.Add(this);
    //    }
    //}


}
