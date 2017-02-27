using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmtSystem.Domain.Shard;

namespace YmtSystem.Domain.Test
{
    public class Order : AggregateRoot
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name不能为空")]
        public string Name { get; set; }
        [Range(0, 100, ErrorMessage = "Price 必须在0,100")]
        public decimal Price { get; set; }
        [Range(0, 5, ErrorMessage = "状态 必须在0,5")]
        public int Status { get; set; }

        public static Order Create(string name, decimal price)
        {
            //TODO:这里完成订单创建逻辑
            return new Order { Name = name, Price = price, Status = 0 };
        }

        public void ChangStatus(int status)
        {
            //TODO:这里完成订单状态逻辑
            this.Status = status;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            //yield return new ValidationResult("验证", new string[] { }),new ValidationResult("验证", new string[] { });

            //写法2：
            var validationResults = new List<ValidationResult>();
            //TODO:这里可以写自定义验证
            if (Price == 50)
                validationResults.Add(new ValidationResult("必须大于50", new string[] { "ChildrenPrice" }));
            return validationResults;
        }
        public Order(string id) 
        {
            this.Id = id;
        }
        protected Order() 
        { }
    }
}
