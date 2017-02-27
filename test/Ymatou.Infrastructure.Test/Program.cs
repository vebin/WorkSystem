using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmtSystem.Infrastructure.Test
{
    class Program
    {
        static void Main(string[] args)
        {
          
            Console.Read();
            var t = new IocTest();
            t.Start();
            t.Show();
            Console.Read();
            t.End();
        }
    }
}
