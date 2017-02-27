using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Autofac.Builder;
using Autofac;

namespace YmtSystem.Infrastructure.Container._Autofac.Test
{
    [TestFixture]
    public class AutofacTest
    {
        private IContainer container;
        [TestFixtureSetUp]
        public void SetUp()
        {
            var c = new ContainerBuilder();
            c.RegisterType<A>().As<IA>();
            c.RegisterGeneric(typeof(B<>)).As(typeof(IB<>));
            container = c.Build();

            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
        public AutofacTest() { }

        [Test]
        public void Show()
        {
            container.Resolve<IA>().Show();
        }
        [Test]
        public void ShowB()
        {
            container.Resolve<IB<_B>>().Show();
        }
        [Test]
        public void Get()
        {
            var url = "http://auth.alpha.ymatou.com/json/reply/SignAuthRequestDto?Token=C087850C7E48AAB3DD6E0C1385BD55B88A0390C53D53E4695816E2040BE233EC261BA806DC26B50FC199E28DA8CB0E5B8228FA5FF6A807FE&ClientIp=::1&sClientType=TurkeyActivity";
            System.Net.HttpWebRequest httprequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httprequest.GetResponse();
            System.IO.Stream steam = response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(steam, Encoding.GetEncoding("gb2312"));
            var result = reader.ReadToEnd();
            reader.Close();


        }
    }
}
