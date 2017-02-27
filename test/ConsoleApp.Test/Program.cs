using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            new Contractdemo().Init(6);
            Console.Read();
            var app = new AsyncApp();
            app.Show2();
            Console.WriteLine("main end ");
            Console.Read();
        }
    }
}
