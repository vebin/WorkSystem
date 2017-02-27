namespace System.Threading.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.Collections.Concurrent.Partitioners;

    public static partial class ParallelAlgorithms
    {
        /// <summary>
        /// 简单map reduce 适合分组统计
        /// </summary>       
        public static IDictionary<T, int> MR<T, K>(this IEnumerable<T> input, Func<T, K> keySelector, Func<int, bool> mrFilter = default(Func<int,bool>), ParallelLinqOptions options = default(ParallelLinqOptions))
        {
            var tmpMap = input.AsParallel().ToLookup(e => e, keySelector);

            if (options == default(ParallelLinqOptions))
                options = new ParallelLinqOptions();

            if (mrFilter != default(Func<int, bool>))
                return
                    tmpMap
                    .AsParallel(options)
                    .Where(e => mrFilter(e.Count()))
                    .ToDictionary(e => e.Key, e => e.AsParallel().Count())
                    .AsParallel(options)
                    .OrderBy(e => e.Value)
                    .ToDictionary(e => e.Key, e => e.Value);
            return
                 tmpMap
                .AsParallel(options)
                .ToDictionary(e => e.Key, e => e.AsParallel().Count())
                .AsParallel(options)
                .OrderBy(e => e.Value)
                .ToDictionary(e => e.Key, e => e.Value);
        }
        /// <summary>
        /// 简单map reduce 适合分组统计 ，PLINQ延迟执行
        /// </summary> 
        public static IDictionary<T, int> LazyMR<T, K>(this IEnumerable<T> input, Func<T, K> keySelector, Func<int, bool> mrFilter = default(Func<int,bool>), ParallelLinqOptions options = default(ParallelLinqOptions))
        {
            var tmpMap = input.AsParallel().ToLookup(e => e, keySelector);

            if (options == default(ParallelLinqOptions))
                options = new ParallelLinqOptions();

            if (mrFilter != default(Func<int, bool>))
                return
                    (from IGrouping<T, int> wordMap in tmpMap.AsParallel(options)
                     where mrFilter(wordMap.Count())
                     select new { Word = wordMap.Key, Count = wordMap.AsParallel().Count() }).ToDictionary(e => e.Word, e => e.Count);
            return
                  (from IGrouping<T, int> wordMap in tmpMap.AsParallel(options)
                   select new { Word = wordMap.Key, Count = wordMap.AsParallel().Count() })
                 .ToDictionary(e => e.Word, e => e.Count);
        }
        /// <summary>
        /// Map
        /// </summary>      
        public static IEnumerable<TPartialResult> M<TInput, TPartialResult>(this IEnumerable<TInput> input
            , Func<IEnumerable<TInput>, TPartialResult> m, ParallelOptions po = default (ParallelOptions), Func<TInput, bool> filter = default(Func<TInput,bool>))
        {
            if (po == default(ParallelOptions)) po = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            var tmpArray = input.AsParallel().ToArray();
            var result = new ConcurrentBag<TPartialResult>();
            var partitionerSize = Environment.ProcessorCount;
            Parallel.ForEach(/*ChunkPartitioner.Create(input, partitionerSize)*/Partitioner.Create(0, input.Count(), partitionerSize)
                , po
                , () => new ConcurrentBag<TPartialResult>()
                , (source, loopState, index, localList) =>
            {
                var tmpresult = new ConcurrentBag<TInput>();
                for (var i = source.Item1; i < source.Item2; i++)
                {
                    if (filter != default(Func<TInput, bool>) && filter(tmpArray[i]))
                        tmpresult.Add(tmpArray[i]);
                    else
                        tmpresult.Add(tmpArray[i]);
                }
                localList.Add(Task.Factory.StartNew(() => m(tmpresult)).Result);
                return localList;
            }
            , e =>
            {
                e.AsParallel().ForAll(v => result.Add(v));
            });
            return result;
        }
        /// <summary>
        /// Rudece
        /// </summary>      
        public static IEnumerable<TResult> R<TResult, TPartialResult>(this IEnumerable<TPartialResult> pr
            , Func<TPartialResult, TResult> r, ParallelOptions po = default (ParallelOptions), Func<TPartialResult, bool> filter = default(Func<TPartialResult,bool>))
        {
            if (po == default(ParallelOptions)) po = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            var result = new ConcurrentBag<TResult>();
            Parallel.ForEach(SingleItemPartitioner.Create(pr)
                , po
                , () => new ConcurrentBag<TResult>()
                , (source, loopState, index, taskFactor) =>
            {
                if (filter != default(Func<TPartialResult, bool>) && filter(source))
                    taskFactor.Add(Task.Factory.StartNew(() => r(source)).Result);
                else
                    taskFactor.Add(Task.Factory.StartNew(() => r(source)).Result);
                return taskFactor;
            }
            , e =>
            {
                e.AsParallel().ForAll(v => result.Add(v));
            });
            return result;
        }

        /// <summary>
        /// map reduce 完成，这只是一个标记方法;completeEndCallback 完成时后阶段工作，可选。
        /// </summary>        
        public static TResult E<TResult>(this TResult r, Func<TResult, TResult> completeEndCallback = default (Func<TResult,TResult>))
        {
            if (completeEndCallback != default(Func<TResult, TResult>))
                return completeEndCallback(r);
            return r;
        }
        #region

        private static Task<TResult> CreateReduceTask<TPartialResult, TResult>
           (Func<TPartialResult[], TResult> reduce,
           Task<TPartialResult>[] mapTasks)
        {
            return Task.Factory.ContinueWhenAll(mapTasks, tasks => PerformReduce(reduce, tasks));
        }

        private static TResult PerformReduce<TPartialResult, TResult>
            (Func<TPartialResult[], TResult> reduce,
            IEnumerable<Task<TPartialResult>> tasks)
        {
            var results = from task in tasks select task.Result;
            return reduce(results.ToArray());
        }


        public static IEnumerable<TInput> M2<TInput>
            (this IEnumerable<TInput> inputs
            , Func<TInput, TInput, TInput> map
            , TInput seed = default (TInput)
            , ParallelOptions op = default (ParallelOptions))
        {
            op = op ?? new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            var tmpTaskList = new ConcurrentBag<TInput>();
            var tmpList = inputs.ToArray();
            TInput tmpSeed = seed;
            Parallel.For(0
                , inputs.Count()
                , op
                , () => tmpSeed
                , (i, loop, local) => { return /*Task.Factory.StartNew(() => */map(tmpList[i], local)/* ).Result*/; }
                , e => tmpTaskList.Add(e));

            return tmpTaskList;
        }
        public static TResult R2<TPartialResult, TResult>(this IEnumerable<TPartialResult> pR
            , Func<IEnumerable<TPartialResult>, TResult> reduce
            , TResult seed
            , ParallelOptions op = default (ParallelOptions)
            )
        {
            op = op ?? new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            var tmpPR = pR.Select(e => e);
         
            return Task.Factory.StartNew(() => reduce(tmpPR), TaskCreationOptions.LongRunning).Result;
        }
        #endregion
    }
}
