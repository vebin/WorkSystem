namespace YmtSystem.Infrastructure.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.CrossCutting.Utility;

    internal class BootStrapper : Disposable
    {
        public bool Execute(out string errorMsg)
        {
            try
            {
                errorMsg = string.Empty;
                BuildManagerWrapper
                     .Current
                     .PublicTypes.Where(e =>
                         e != null
                         && !e.FullName.Contains("BootstrapperTask")
                         && typeof(BootstrapperTask).IsAssignableFrom(e))
                         .Each(type =>
                         {
                             ((BootstrapperTask)Activator.CreateInstance(type)).Execute();
                         });

                return true;
            }
            catch(Exception ex)
            {
                errorMsg = ex.ToString();
                return false;
            }
        }

        protected override void InternalDispose()
        {
            BuildManagerWrapper
                 .Current
                 .PublicTypes.Where(e =>
                     e != null
                     && !e.FullName.Contains("BootstrapperTask")
                     && typeof(BootstrapperTask).IsAssignableFrom(e))
                     .Each(type =>
                     {
                         ((BootstrapperTask)Activator.CreateInstance(type)).Dispose();
                     }, errorHandler: ex =>
                     {

                     });
        }
    }
}
