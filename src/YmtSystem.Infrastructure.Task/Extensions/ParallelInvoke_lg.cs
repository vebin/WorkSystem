using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Threading.Tasks
{
    public class ParallelInvoke
    {
        public void TryInvoke(ParallelOptions parallelOptions, params Action[] actions)
        {
            try
            {
                Parallel.Invoke(parallelOptions, actions);
            }
            catch (AggregateException ex)
            {

            }
        }
    }
}
