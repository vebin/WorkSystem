using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.Test
{
    public class AsyncApp
    {
        public void Show()
        {
            Console.WriteLine(1 + "->" + Thread.CurrentThread.ManagedThreadId);
        }

        public async Task Show1()
        {
            //await 等待异步方法完成，如果不使用await则 不能保证异步执行顺序
            var r1 = await Task.Run(() => { Thread.Sleep(1000); Console.WriteLine(2 + "->" + Thread.CurrentThread.ManagedThreadId); return 2; });
            var r2 = await Task.Run(() => { Thread.Sleep(1000); Console.WriteLine(3 + "->" + Thread.CurrentThread.ManagedThreadId); return 3; });
            var r3 = await Task.Run(() => { Thread.Sleep(1000); Console.WriteLine(4 + "->" + Thread.CurrentThread.ManagedThreadId); return 4; });
            var r4 = await Task.Run(() => { Thread.Sleep(1000); Console.WriteLine(5 + "->" + Thread.CurrentThread.ManagedThreadId); return 5; });
            Thread.Sleep(1000);
            Console.WriteLine("全部执行完成 结果 :{0},{1},{2},{3}", r1, r2, r3, r4);
        }

        public async Task Show2()
        {
            //await 等待异步方法完成，如果不使用await则 不能保证异步执行顺序
            var r1 = await Task.Run(() => { Thread.Sleep(1000); Console.WriteLine(2 + "->" + Thread.CurrentThread.ManagedThreadId); return 2; });
            var r2 = await Task.Run(() => { Thread.Sleep(1000); Console.WriteLine(3 + "->" + Thread.CurrentThread.ManagedThreadId); return 3; });
            var r3 = await Task.Run(() => { Thread.Sleep(1000); Console.WriteLine(4 + "->" + Thread.CurrentThread.ManagedThreadId); return 4; });
            var r4 = await Task.Run(() => { Thread.Sleep(1000); Console.WriteLine(5 + "->" + Thread.CurrentThread.ManagedThreadId); return 5; });
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.WriteLine("6->"+Thread.CurrentThread.ManagedThreadId);
            });
            //Thread.Sleep(1000);
            Console.WriteLine("全部执行完成 结果 :{0},{1},{2},{3}", r1, r2, r3, r4);
        }
    }
}
