using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymt.Trading.Domain.Repository.Mapping;
using YmtSystem.Repository.EF;
using YmtSystem.Repository.EF.DBAttribute;

namespace Ymt.Trading.Domain.Repository.TradingContext
{
    [StoreContextAttribute]
    public class YmtTradingUnitOfwork : EFUnitOfWork
    {
        public YmtTradingUnitOfwork(string contextName)
            : base(contextName)
        {
            Console.WriteLine("ssss");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new YmtOurderMappingConfigure());
            base.OnModelCreating(modelBuilder);
        }
        //protected override void Dispose(bool disposing)
        //{
        //    Console.WriteLine("ffffffffff");
        //    base.Dispose(disposing);
        //}
    }
   
}
