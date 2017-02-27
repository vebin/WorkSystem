using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Autofac.Builder;
using Autofac;
using YmtSystem.Infrastructure.Container.AutofacMvc.Controllers;
using System.Reflection;

namespace YmtSystem.Infrastructure.Container.AutofacMvc
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new Autofac.ContainerBuilder();

            AppDomain.CurrentDomain.GetAssemblies().Where(e => !e.GlobalAssemblyCache).ToList().ForEach(e =>
            {
                if (typeof(Controller).IsAssignableFrom(e.GetType()))
                {
                    builder.RegisterControllers(e);
                }
            });

            builder.RegisterType<ContainerTest>().As<IContainerTest>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}