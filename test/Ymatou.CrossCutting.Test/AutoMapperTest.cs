using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Infrastructure.AutoMapperAdapter;
using AutoMapper;
using AutoMapper.Mappers;
using NUnit.Framework;

namespace YmtSystem.CrossCutting.Test
{
    public enum TT
    {
        H = 0, F = 1
    }
    public class A
    {
        public string Field5 { get; set; }
        public DateTime Field2 { get; set; }
        public int Field3 { get; set; }
        public A() { }
        public A(int v) { this.Field3 += v; }

    }
    public class C
    {
        public int A { get; set; }
        public D DT { get; set; }
    }

    public class D
    {
        public TT Dt { get; set; }
    }
    public class H
    {
        public int HA { get; set; }
        public int HD { get; set; }
    }
    public class B
    {
        public string Field { get; set; }
        public DateTime Field2 { get; set; }
        public int Field3 { get; set; }
        public string Field4 { get; set; }
        public void MappBefor()
        {
            Console.WriteLine("映射前执行");
        }
        public B()
        {
            Field = "A";
            Field2 = DateTime.Now;
            Field3 = 1;
            Field4 = "OK";
        }
    }
    public class Formatter : IValueFormatter
    {
        public string FormatValue(ResolutionContext context)
        {
            return string.Format("{0}_", context.DestinationValue);
        }
    }
    public class DateConvert : TypeConverter<string, DateTime>
    {

        protected override DateTime ConvertCore(string source)
        {
            return System.Convert.ToDateTime(source);
        }
    }
    public class DateConvert2 : TypeConverter<B, A>
    {

        protected override A ConvertCore(B source)
        {
            return new A { Field2 = System.Convert.ToDateTime(source.Field4) };
        }
    }
    [TestFixture]
    public class AutoMapperTest
    {
        [SetUp]
        public void Setup()
        {
            //重置所有设置
            Mapper.Reset();
        }
        [Test]
        [Description("嵌套")]
        public void Mapp_Nested()
        {
            var newH = YmtEntityMappingDSL
                                 .CreateMap<C, H>(true)
                
                                 .ForMember(e => e.HA, _e => _e.MapFrom(__ => __.A))
                                 //.ForMember(e => e.HD, _e => _e.MapFrom(__ => __.DT.Dt))
                                 .ForSourceMember(e => e.DT, e => e.Ignore())
                                 .MapTo(new C { A = 100, DT = new D { Dt = TT.F } });

            Assert.AreEqual(100, newH.HA);
            Assert.AreEqual(1, newH.HD);
        }
        [Test]
        [Description("字段全匹配映射")]
        public void Mapp_1to1_Test()
        {
            var newB = new B();
            //默认规则是目标对象字段去查找源对象字段
            var a = newB.MapTo<B, A>();
            Assert.AreEqual(null, null);
            Assert.AreEqual(1, a.Field3);
        }
        [Test]
        [Description("自定义单个字段映射")]
        public void Mapp_Member_Test()
        {

            var map = Mapper.CreateMap<B, A>().ForMember(e => e.Field5, e => e.MapFrom(b => b.Field4));
            var a = Mapper.Map<B, A>(new B());
            Assert.AreEqual("OK", a.Field5);
            Assert.AreEqual(1, a.Field3);
        }
        [Test]
        [Description("映射前执行")]
        public void Mapp_Befor_Test()
        {
            Mapper.CreateMap<B, A>().BeforeMap((b, a) => b.Field3 = 3).ForMember(a => a.Field3, b => b.MapFrom(_b => _b.Field3)).ForMember
                (a => a.Field5, b => b.MapFrom(_b => _b.Field4));
            var _a = Mapper.Map<B, A>(new B());
            Assert.AreEqual(3, _a.Field3);
            Assert.AreEqual("OK", _a.Field5);
        }
        [Test]
        [Description("")]
        public void Mapp_IgnoreMember_Test()
        {
            Mapper.CreateMap<B, A>().ForMember(a => a.Field2, b => b.Ignore());
            var _a = Mapper.Map<B, A>(new B());
            Assert.AreEqual(DateTime.MinValue, _a.Field2);
        }
        [Test]
        [Description("格式化")]
        public void Mapp_Formater_Test()
        {
            Mapper.AddFormatter(new Formatter());
            Mapper.CreateMap<B, A>().ForMember(a => a.Field5, b => b.MapFrom(_b => _b.Field4));
            var _a = Mapper.Map<B, A>(new B());
            Assert.AreEqual("_", _a.Field5);
        }
        [Test]
        [Description("类型转换")]
        public void Mapp_TypeConvert_Test()
        {
            var dtNow = DateTime.Now;
            var dt = dtNow.ToString();
            Mapper.CreateMap<string, DateTime>().ConvertUsing<DateConvert>();
            Mapper
                .CreateMap<B, A>()
                .ForMember(a => a.Field2, b => b.MapFrom(_b => _b.Field4));
            var _a = Mapper.Map<B, A>(new B { Field4 = dt });
            Assert.AreEqual(dtNow.Second, _a.Field2.Second);
        }
        [Test]
        [Description("configure")]
        public void Mapp_Config_Test()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<string, DateTime>().ConvertUsing<DateConvert>();
                cfg.CreateMap<B, A>()
                .ForMember(a => a.Field2, b => b.MapFrom(_b => _b.Field4));
            });
            var dtNow = DateTime.Now;
            var dt = dtNow.ToString();
            var _a = Mapper.Map<B, A>(new B { Field4 = dt });
            Assert.AreEqual(dtNow.Second, _a.Field2.Second);
        }
        [Test]
        [Description("Construct")]
        public void Mapp_Construct_Test()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.DisableConstructorMapping();//禁用构造函数映射
                cfg
                    .CreateMap<B, A>()
                    .ConstructUsing(b => new A(b.Field3 + 1))//构造函数映射
                    .ForMember(a => a.Field3, b => b.MapFrom(_b => _b.Field3));
            });

            var _a = Mapper.Map<B, A>(new B { Field3 = 4 });
            Assert.AreEqual(4, _a.Field3);
        }
        [Test]
        public void Mapp_YmtMapp_Test()
        {
            var dt = DateTime.Now;
            var v = dt.ToString();
            var _a = YmtEntityMappingDSL
                                .CreateMap<B, A>(true)
                                .TypeConvert(b => new A() { Field2 = Convert.ToDateTime(b.Field4) })
                                .ForMember(a => a.Field3, b => b.MapFrom(_b => _b.Field3))
                                .ForMember(a => a.Field2, b => b.MapFrom(_b => _b.Field4))
                                .MapTo(new B { Field3 = 10, Field2 = dt, Field4 = "S" });
            //YmtSystemAssert.AssertArgumentEquals(_a,null, "映射失败");
            Assert.AreEqual(dt.Second, _a.Field2.Second);
        }


    }
}
