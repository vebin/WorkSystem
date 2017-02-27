using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymt.Trading.Domain.Model.Table
{
    public class T12 : T11
    {
        public int T121 { get; set; }
        public string T122 { get; set; }

        public T12(string id) 
        {
            this.Id = id;
        }
        protected T12() { }
    }
}
