
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Threading.Algorithms
{
    public static partial class ParallelAlgorithms
    {
        public static int ParallelSum(this IEnumerable<int> source, Func<int, int> val, TimeSpan timeOut = default(TimeSpan), bool timeOutThrow = false)
        {
            var returnSum = 0;
            
            var p = Parallel.For<int>(0
                , source.Count()
                , new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }
                , () => 0
                , (i, loop, subTotal) =>
                                    subTotal += val(i)
                , s => Interlocked.Add(ref returnSum, s));

            if (timeOut != default(TimeSpan))
                if (!SpinWait.SpinUntil(() => p.IsCompleted, timeOut))
                    if (timeOutThrow)
                        throw new TimeoutException("计算超时");

            return returnSum;
        }
    }
}
