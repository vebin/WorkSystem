using System.Web;
using System.Web.Mvc;

namespace YmtSystem.Infrastructure.Container.AutofacMvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}