using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YmtSystem.Infrastructure.Container._Autofac.Test
{
    public interface IA
    {
        void Show();
    }

    public class A : IA
    {

        public void Show()
        {
            Console.WriteLine("ssssssss");
        }
    }

    public interface IB<T>
    {
        void Show();
    }

    public class _B
    {
    }

    public class B<T> : IB<T>
    {
        private readonly IA ia;

        public B(IA ia)
        {
            this.ia = ia;
        }

        public void Show()
        {
            Console.WriteLine(typeof(T).FullName + "->" + ia.ToString());
        }
    }
}
