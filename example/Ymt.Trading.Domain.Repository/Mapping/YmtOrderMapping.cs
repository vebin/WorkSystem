namespace Ymt.Trading.Domain.Repository.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using YmtSystem.Repository.EF.ModelMapping;
    using Ymt.Trading.Domain.Model.Order;

    public class YmtOurderMappingConfigure : EntityTypeConfiguration<YmtOrder>
    //, IDBMappingRegister
    {
        public YmtOurderMappingConfigure()
        {
            //TODO: 这里完成映射
            ToTable("ymt_trading_order_2");
            HasKey(e => e.Id);
            //Ignore(e => e.Version);
            Property(e => e.RAddress.City).HasColumnName("City").IsOptional();
            Property(e => e.RAddress.Street).HasColumnName("Street").IsOptional();
            Property(e => e.RAddress.Zip).HasColumnName("Zip").IsOptional();
            //HasMany 1对多；一个订单对应多个可选的订单项 ，Order_line_Id 即为在订单项表的外键名称
            HasMany(e => e.OrderLineList).WithOptional().Map(_ => _.MapKey("Order_line_Id")).WillCascadeOnDelete();

            //Console.WriteLine("。。。。。。。。。order 映射被执行");
        }

        //public void Register(ConfigurationRegistrar cfg)
        //{
        //    cfg.Add(this);
        //}
    }
}
