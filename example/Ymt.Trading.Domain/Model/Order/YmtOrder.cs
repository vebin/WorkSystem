namespace Ymt.Trading.Domain.Model.Order
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ymt.Trading.Domain.Event;
    using YmtSystem.Domain;
    using YmtSystem.Domain.Event;
    using YmtSystem.Domain.Shard;

    public class YmtOrder : AggregateRoot, IConcurrencyCheck, IEntityLogicDelete,IEntityExtend
    {
        public bool IsDelete { get; set; }
        [Timestamp] 
        public byte[] Version { get; set; }
        private HashSet<OrderLine> orderLine = new HashSet<OrderLine>();

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name不能为空")]
        public string Name { get; set; }
        [Range(0, 10000, ErrorMessage = "Price 必须在0,10000")]
        public decimal Price { get; set; }
        [Range(0, 5, ErrorMessage = "状态 必须在0,5")]
        public int Status { get; set; }
        public string OrderName { get; set; }
        public ReceiptAddress RAddress { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public HashSet<OrderLine> OrderLineList { get { return orderLine; } set { orderLine = value; } }
        protected YmtOrder() 
        {
        }
        public YmtOrder(string id, string name, decimal price) 
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.CreateTime = DateTime.Now;
            this.IsDelete = false;
        }
        public static YmtOrder Create(string name, decimal price)
        {
            //TODO:这里完成订单创建逻辑
            return new YmtOrder { Id = "12", Name = name, Price = price, Status = 0 };
        }
        public YmtOrder AddOrderLine(OrderLine line)
        {
            orderLine.Add(line);
            return this;
        }
        public YmtOrder SetAddress(ReceiptAddress address)
        {
            this.RAddress = address;
            return this;
        }
        public void OrderCreateEvent()
        {
            //TODO:这里完成领域事件
            //DomainEvent.Publish<OrderCreateEvent>(new OrderCreateEvent(this) { });
        }

        public void ChangStatus(int status)
        {
            //Contract.Requires(status == 5, "状态不是有效的值");
            //TODO:这里完成订单状态逻辑
            this.Status = status;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            //TODO:这里可以写自定义验证
            if (Price == 50)
                validationResults.Add(new ValidationResult("必须大于50", new string[] { "ChildrenPrice" }));
            return validationResults;
        }

        protected override IEnumerable<object> CompareComponents()
        {
            yield return this.Id;
            yield return this.Name;
        }
    }
}
