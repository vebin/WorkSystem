namespace Ymt.Trading.Domain.Model.Order
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YmtSystem.Domain.Shard;

    public class OrderLine : Entity,IConcurrencyCheck ,IEntityLogicDelete
    {
        public decimal ItemPrice { get; set; }

        public OrderLine(decimal price) 
        {
            this.Id = Guid.NewGuid().ToString("N");
            this.ItemPrice = price;
            this.IsDelete = false;
         
        }

        protected OrderLine() 
        {
        }

        public byte[] Version
        {
            get
           ;
            set
           ;
        }

        public bool IsDelete
        {
            get
           ;
            set
          ;
        }
    }
}
