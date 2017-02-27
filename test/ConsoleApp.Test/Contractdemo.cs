using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test
{
    public class Contractdemo
    {
        public int Price { get; set; }
        public int Init(int price)
        { 
            Contract.Requires(price > 0, "price必须大于零");           
            Contract.Ensures(this.Price > 10, "结果错误");
            
            this.Price = price;
            this.Price = this.Price + 5;                    
            return this.Price;
        }

        public int Add()
        {
            return 10;
        }
    }
}
