using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Async;
using System.Threading.Algorithms;
using ParallelExtensionsExtras;

using System.IO;
using System.Net;
using System.Numerics;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace ParallelExtensionsExtras.Test
{
    /// <summary>
    /// ParallelExtensionsExtras 的摘要说明
    /// </summary>
    [TestFixture]
    public class ParallelExtensionsExtras
    {
        #region [ other ]
        public ParallelExtensionsExtras()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [Test]
        public void TestMethod1()
        {
            //
            // TODO: 在此处添加测试逻辑
            //
        }
        #endregion
        #region   [ Extensions->LazyExtensions ]
        [Test]
        public void Test_LazyExtensions_Force()
        {
            Lazy<int> lazy = new Lazy<int>(() => 1);
            var result = lazy.Force().GetValueAsync().Result;
            Assert.AreEqual(1, result);
        }
        [Test]
        public void Test_LazyExtensions_Create()
        {
            var result = LazyExtensions.Create<int>(5).Value;
            Assert.AreEqual(5, result);
        }
        #endregion
        #region [ Extensions->APM->FileAsync ]
        [Test]
        public void Test_FileAsync_Read()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\a.txt";
            var result = FileAsync.ReadAllText(path).Result;
            Assert.IsNotNull(result);
        }
        [Test]
        public void Test_FileAsync_Write()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\a.txt";
            var task = FileAsync.WriteAllText(path, "测试");
            SpinWait.SpinUntil(() => task.IsCompleted);
            Assert.AreEqual(true, task.IsCompleted);
        }
        [Test]
        public void Test_FileAsync_Write2()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\a.txt";
            var task = FileAsync.WriteAllText(path, "测试").ReturnTaskCompleted(TimeSpan.FromSeconds(5), true).Item1;
            Assert.AreEqual(true, task.IsCompleted);
        }
        #endregion
        #region  [ Extensions->EAP->WebClientExtensions ]
        [Test]
        public void Test_WebClientExtensions_Request()
        {
            var token11 = new CancellationTokenSource();
            var t22 = token11.Token;
            t22.Register(_ =>
            {
                Console.WriteLine("被取消了。。。");
            }, null);
            var webRequest = WebRequest.Create("http://www.sina.com.cn/");
            webRequest.Method = "GET";
            try
            {
                //任务取消并且调用 Result  怎会抛出异常，任务取消不调用result 则不会抛出异常
                var result2 = webRequest.GetResponseAsync().ReturnTaskCompleted(TimeSpan.FromSeconds(0.1), token11).Item1.Result;
                var rStream = new StreamReader(result2.GetResponseStream());
                var readLine = rStream.ReadLine();
                Assert.IsNotNull(readLine);
            }
            catch (AggregateException ex)
            {
                ex.InnerExceptions.EachHandle(exxx =>
                {
                    Console.WriteLine(exxx.ToString());
                    Console.WriteLine();
                });

            }
        }
        [Test]
        public void Test_WebClientExtensions_Request_Download()
        {
            //var path = AppDomain.CurrentDomain.BaseDirectory + "\\b.txt";
            //var webRequest = WebRequest.Create("http://www.sina.com.cn/");
            //webRequest.Method = "GET";
            //var result = webRequest.DownloadDataAsync().EnsureTaskCompleted().Result;
            //var stream = new MemoryStream(result);

            //var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            //stream.WriteTo(fs);
            //fs.Close();
            //stream.Close();
            Assert.IsTrue(true);
        }
        #endregion
        #region   [ Extensions->APM->StreamExtensions ]
        [Test]
        public void Test_StreamExtensions_WriteToFile()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\c.txt";
            var byter = System.Text.ASCIIEncoding.GetEncoding("utf-8").GetBytes("ssssssssssssssssssdfs");
            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            var result = fs.WriteAsync(byter, 0, byter.Length).EnsureTaskCompleted().IsCompleted;
            fs.Close();

            Assert.IsTrue(result);
        }
        [Test]
        public void Test_StreamExtensions_ReadByte()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\a.txt";
            var value = "测试s";
            var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            var readResult = fs.ReadAllBytesAsync().EnsureTaskCompleted().Result;
            fs.Close();
            var readString = System.Text.ASCIIEncoding.GetEncoding("utf-8").GetString(readResult);

            Assert.AreEqual(value, readString);

        }
        #endregion
        #region [ Extensions->TaskFactoryExtensions_Create ]
        [Test]
        public void Test_TaskFactoryExtensions_Create_CreateTask()
        {
            var task = Task.Factory.Create<int>(e => 5, 5);
            task.Start();
            var result = task.EnsureTaskCompleted().Result;
            Assert.AreEqual(5, result);
        }
        #endregion
        #region [ Extensions->TaskFactoryExtensions_Delayed ]
        [Test]
        public void Test_TaskFactoryExtensions_Delayed_TaskStart()
        {
            var task = Task<string>.Factory.StartNewDelayed(3000, e => e.ToString(), "delayed start task").EnsureTaskCompleted();

            Assert.IsTrue(task.IsCompleted);
            Assert.AreEqual("delayed start task", task.Result);
        }
        [Test]
        public void Test_TaskFactoryExtensions_Delayed_TaskStart_Cancell()
        {
            //var cancell = new System.Threading.CancellationTokenSource();
            //var cancellToken = cancell.Token;
            //var task = Task<String>.Factory.StartNewDelayed(4000, e => e.ToString(), "cancell", cancellToken);
            //Thread.Sleep(2000);
            ////
            ////cancell.Cancel();
            ////
            //Assert.IsFalse(task.IsCanceled);
            //Assert.IsTrue(task.IsCompleted);
            //Assert.AreEqual("cancell", task.Result);
        }
        #endregion
        #region [ Extensions->TaskFactoryExtensions_Iterate ? ]
        public void Test_TaskFactoryExtensions_Iterate_taskIterate()
        {
            var task = Task.Factory.Iterate(new List<string> { "a", "b", "c" });

            Assert.AreEqual(TaskStatus.Running, task.Status);

        }
        #endregion
        #region [ Extensions->TaskFactoryExtensions_TrackedSequence ? ]
        [Test]
        public void Test_TaskFactoryExtensions_TrackedSequence_Task_TrackedSequence()
        {
            //var task = Task
            //    .Factory
            //    .TrackedSequence(new Func<Task>[] { () => new Task(e => e.ToString(), "1"), () => new Task(e => e.ToString(), "2") });
            // task.Start();
            // task.EnsureTaskCompleted();
            // Assert.IsTrue(task.IsCompleted);

            //var resultTaskList = task.Result;
            //resultTaskList.ToList().ForEach(e => Assert.IsTrue(e.IsCompleted));
        }
        #endregion
        #region   [ Extensions->AggregateExceptionExtensions ]
        [Test]
        public void Test_Extensions_AggregateExceptionExtensions_Agg()
        {
            //AggregateException 异常扩展
            Assert.IsTrue(true);
        }
        #endregion
        #region   [ Extensions->BlockingCollectionExtensions ]
        [Test]
        public void Test_Extensions_BlockingCollectionExtensions_Collections_Partitioner()
        {
            var blockCollections = new BlockingCollection<int>(100);
            //集合观察者
            var s = new ObserverTestImp<int>(e => { }, e => { }, () => { });
            blockCollections.AddFromObservable<int>(s, false);
            blockCollections.Add(1);

            Assert.IsTrue(true);
        }
        #endregion
        #region   [ Extensions->PlinqExtensions & mr ]
        [Test]
        public void Test_Extensions_ParallelLinqExtensions_Top()
        {
            var list = new List<int> { 1, 2, 3, 4, 54643, 6, 67, 2, 3, 5, 0, 45, -9, 100, 5 };
            //获取序列top x 元素  升序排列
            var result = list.AsParallel().TakeTop(e => e, 5);

            Assert.AreEqual(5, result.Count());

        }
        [Test]
        public void Test_Extensions_ParallelLinqExtensions_MapReduceWord()
        {
            //PLINQ MR 统计简单单词个数 
            var list = "a bb a bb cc Beim ersten Aufruf von FailMethod von FailMethod".Split(new char[] { ' ' });

            var result = list.AsParallel().MapReduce(
                  e => new List<string>() { e }
                , e => e
                , e => new[] { new KeyValuePair<string, int>(e.Key, e.Count()) });

            Assert.AreEqual(8, result.Count());
            Assert.AreEqual(2, result.ToDictionary(e => e.Key, e => e.Value)["a"]);
        }
        [Test]
        public void Test_Extensions_ParallelLinqExtensions_MapReduceFileWord()
        {
            //PLINQ MR 复杂统计文件单词个数 
            var filePath = AppDomain.CurrentDomain.BaseDirectory;
            var files = Directory.EnumerateFiles(filePath, "*.txt").AsParallel();
            var counts = files.MapReduce
                (
                path => File.ReadLines(path, Encoding.GetEncoding("utf-8")).SelectMany(line => line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)),
                word => word,
                group => new[] { new KeyValuePair<string, int>(group.Key, group.Count()) }
            );
            Assert.IsTrue(counts.Count() > 0);
        }
        #endregion
        #region   [ ParallelAlgorithms->ParallelAlgorithms_MapReduce_lg ]
        //[TestMethod]
        public void Test_ParallelAlgorithms_MapReduce_lg_DiskFileWordCountTest()
        {
            //MR 统计磁盘文件的单词并分组
            var filePathMR = AppDomain.CurrentDomain.BaseDirectory;
            var filesMR = Directory.EnumerateFiles(filePathMR, "*.txt").AsParallel();
            var countsMR = filesMR
            .M(path =>
                    {
                        var gr = new List<IEnumerable<IGrouping<string, string>>>();
                        path.ToList().ForEach(e =>
                        {
                            var rL = File.ReadAllLines(e, Encoding.GetEncoding("utf-8")).SelectMany(line => line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                            gr.Add(rL.GroupBy(k => k));
                        });
                        return gr;
                    })
            .R(e =>
                    {
                        var listR = new List<KeyValuePair<string, int>>();
                        foreach (var _ in e)
                            foreach (var __ in _)
                                listR.Add(new KeyValuePair<string, int>(__.Key, __.Count()));
                        return listR;
                    })
            .E();
            //foreach (var ee in countsMR)
            //    foreach (var e in ee)
            //        Console.WriteLine(e.Key + " -> " + e.Value);

            Assert.IsTrue(true);
        }
       [Test]
        public void Test_ParallelAlgorithms_MapReduce_lg_WordCount()
        {
            var listWord = "a bb a bb cc Beim ersten Aufruf von FailMethod von FailMethod".Split(new char[] { ' ' });
            var mrResult = listWord
                                    .M(e => e.GroupBy(k => k))
                                    .R(e =>
                                    {
                                        var listR = new List<KeyValuePair<string, int>>();
                                        foreach (var _ in e)
                                            listR.Add(new KeyValuePair<string, int>(_.Key, _.Count()));
                                        return listR;
                                    })
                                    .E();
            //foreach (var e in mrResult)
            //{
            //    foreach (var ee in e)
            //        Console.WriteLine(ee.Key + " -> " + ee.Value);
            //}
            Assert.IsTrue(true);
        }
       [Test]
        public void Test_ParallelAlgorithms_MapReduce_lg_AddValue()
        {
            var listAdd = new List<int> { 1, 2, 3, 4, 5, 6, 76, 87, 9, 10 };
            var addResult = listAdd.M(e => new List<int> { e.AsParallel().Sum() })
                                   .R(e => e.AsParallel().Sum())
                                   .M(e => e.AsParallel().Sum())
                                   .R(e => e)
                                   .E();

            Assert.AreEqual(203, addResult.FirstOrDefault());
        }
       [Test]
        public void Test_ParallelAlgorithms_MapReduce_lg_MaxValue()
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 76, 87, 9, 10, 0, 98, 23, 5, 12, 3453, 34, 56, 6, 23, 1, 34, 89 };
            var maxValue = list.M(e => new List<int> { e.Max() })
                               .R(e => e.Max())
                               .M(e => new List<int> { e.Max() })
                               .R(e => e.Max())
                               .E(e => new List<int> { e.AsParallel().Max() });

            Assert.AreEqual(3453, maxValue.FirstOrDefault());
        }
       [Test]
        public void Test_ParallelAlgorithms_MapReduce_lg_MinValue()
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 76, 87, 9, 10, 0, 98, 23, 5, 12, 3453, 34, 56, 6, 23, 1, 34, 89 };
            var maxValue = list.M(e => new List<int> { e.Min() })
                               .R(e => e.Min())
                               .M(e => new List<int> { e.Min() })
                               .R(e => e.Min())
                               .E(e => new List<int> { e.AsParallel().Min() });

            Assert.AreEqual(0, maxValue.FirstOrDefault());
        }
        #endregion
        #region   [ Extensions->TaskExtrasExtensions ]
        [Test]
        public void Test_TaskExtrasExtensions_taskContinueWith()
        {
            //任务延续 上一个任务完成后的结果 作为下一个任务开始的参数，依次类推
            var task1 = Task<string>
                .Factory
                .StartNew(() => "a")
                .ContinueWith(e => e.Result + "b", TaskContinuationOptions.NotOnCanceled)
                .ContinueWith(e => e.Result + "c", TaskContinuationOptions.NotOnCanceled)
                .EnsureTaskCompleted();

            Assert.AreEqual("abc", task1.Result);
        }
        [Test]
        public void Test_TaskExtrasExtensions_AttachToParent()
        {
            //任务延续 上一个任务完成后的结果 作为下一个任务开始的参数，依次类推
            var task1 = Task<string>
                .Factory
                .StartNew(() => "d")
                .ContinueWith(e => e.Result + "e", TaskContinuationOptions.NotOnCanceled)
                .ContinueWith(e => e.Result + "f", TaskContinuationOptions.NotOnCanceled)
                .AttachToParentAndReturnTask()
                .IgnoreExceptions();

            Assert.AreEqual("def", task1.Result);
        }
       [Test]
        public void Test_TaskExtrasExtensions_WithTimeOut()
        {
            //任务等待超时 WithTimeout 参数timeOut是定时器timer调用回调函数的延时时间
            var task1 = Task<string>
                .Factory
                .StartNew(() => { Thread.Sleep(3); return "d"; })
                .WithTimeout(TimeSpan.FromSeconds(0));

            Assert.AreEqual(TaskStatus.RanToCompletion, task1.Status, "任务已取消");
        }
        [Test]
        public void Test_TaskExtrasExtensions_Then()
        {
            //第一个任务完成后执行第二个任务
            var task1 = Task<string>
                .Factory
                .StartNew(() => { Thread.Sleep(3); return "d"; })
                .Then(() => Task<string>
                .Factory
                .StartNew(() => { return "f"; }));

            Assert.AreEqual("f", task1.Result);
        }
        [Test]
        public void Test_TaskExtrasExtensions_AsyncCallback()
        {
            //第一个任务完成后异步执行第二个任务，返回第一个任务的结果
            var task1 = Task<string>
                .Factory
                .StartNew(() => { Thread.Sleep(3); return "d"; })
                .ToAsync(o =>
                        {
                            //第一个任务及其第一任务返回值 "x"
                            var t = (o as Task<string>);
                            var result = t.Result;
                            //这里开始第二个任务
                            var t2 = Task.Factory.StartNew<string>(() => "f" + result + o.AsyncState).EnsureTaskCompleted();
                            Assert.AreEqual("fdx", t2.Result);
                        }, "x");

            Assert.AreEqual("d", task1.Result);
        }
        #endregion
        #region   [ Extensions->LinqToTasks ]
        [Test]
        public void Test_Extensions_LinqToTasks_LinQToTask_SelectMany()
        {
            //连接多个task 内部为第一个task ContinueWith 完成后执行第二个task
            var task = Task<string>
                                .Factory
                                .StartNew(() => "hell word")
                                .SelectMany(e => Task<string>.Factory.StartNew(() => e + " as"));

            Assert.AreEqual("hell word as", task.Result);
        }
        [Test]
        public void Test_Extensions_LinqToTasks_LinQToTask_Join()
        {
            //内联两个或多个task ,如果内部task 和外部task返回结果不一致，则取消并抛出异常
            var task = Task<string>
                                .Factory
                                .StartNew(() => "hell ")
                                .Join(Task<string>.Factory.StartNew(() => "word")
                                , e => "as"
                                , e => "as"
                                , (e, e2) => e + e2);

            Assert.AreEqual("hell word", task.Result);
        }
        [Test]
        public void Test_Extensions_LinqToTasks_LinQToTask_OrderBy()
        {
            //执行task ，结果排序
            var task = Task<string>
                                .Factory
                                .StartNew(() => "hell word")
                                .OrderBy(e => e);

            Assert.AreEqual("hell word", task.Result);
        }
       [Test]
        public void Test_Extensions_LinqToTasks_LinQToTask_OrderByDescending()
        {
            //执行task ，结果排序
            var task = Task<string>
                                .Factory
                                .StartNew(() => "hell word")
                                .OrderByDescending(e => e);

            Assert.AreEqual("hell word", task.Result);
        }
       [Test]
        public void Test_Extensions_LinqToTasks_LinQToTask_GroupBy()
        {
            //执行task ，结果分组   ????? 有点问题
            var task = Task<string>
                                .Factory
                                .StartNew(() => "hell word hell")
                                .GroupBy(e => e.Split(new char[] { ' ' }), e => e.Split(new char[] { ' ' }).Length);

            Assert.AreEqual(3, task.Result.Key.Count());

        }
        #endregion
        #region [ ParallelAlgorithms->ParallelAlgorithms_Filter ]
        [Test]
        public void Test_ParallelAlgorithms_Filter_Task()
        {
            var list = new List<int> { 1, 1, 2, 3, 4, 5, 6, 1, 9, 10, 23, 3, 19, 15, 4 };
            var resultList = list.Filter(e => e % 2 == 0);
            //resultList.ToList().ForEach(e => Assert.AreEqual(2, e));
            var list2 = new List<int> { 2, 4, 6, 10, 4 };
            //CollectionAssert.AreEqual(list2, resultList);
            Assert.AreEqual(list2.Count, resultList.Count);
        }
        #endregion
        #region [ ParallelAlgorithms->ParallelAlgorithms_For ]
        [Test]
        public void Test_ParallelAlgorithms_For_Add()
        {
            ParallelAlgorithms.For(0, 1000, e => e++);
            Assert.IsTrue(true);
        }
        #endregion
        #region [ ParallelAlgorithms->ParallelAlgorithms_ForRange ]
        [Test]
        public void Test_ParallelAlgorithms_ForRange_Add()
        {
            var start = 0;
            var loopResutl = ParallelAlgorithms
                //这里 e1，e2 分别为数据元素 ，e3 为 local初始变量
                .ForRange<int>(0, 5, () => start, (e1, e2, stat, e3) => e3 = e1 + e2, e => { start = e; });
            Assert.AreEqual(start, start);
        }
        #endregion
        #region [ ParallelAlgorithms->ParallelAlgorithms_Map & ParallelAlgorithms_Reduce 待扩展 ]
        [Test]
        public void Test_ParallelAlgorithms_MapReduce_MR()
        {
            var list = "a bb a bb cc Beim ersten Aufruf von FailMethod von FailMethod".Split(new char[] { ' ' }).ToList();
            //MR 分组求和 实时
            //mr filter 可以指定条件，例如：去重 e => e==1
            var mrResult = list.MR(k => 1, e => e > 0);

            Assert.AreEqual(2, mrResult["a"]);
            Assert.AreEqual(2, mrResult["bb"]);
        }
        [Test]
        public void Test_ParallelAlgorithms_MapReduce_Lazy_MR()
        {
            var list = "a bb a bb cc Beim ersten Aufruf von FailMethod von FailMethod".Split(new char[] { ' ' }).ToList();
            //MR 分组求和 延时，例如：去重 e => e==1
            var mrResult = list.LazyMR(k => 1, e => e > 0);

            Assert.AreEqual(2, mrResult["a"]);
            Assert.AreEqual(2, mrResult["bb"]);
        }
        [Test]
        public void Test_ParallelAlgorithms_Map_ParallelAlgorithms_Reduce_ArrayMaxOrMinValue_MR()
        {
            //MR 求数组中最值
            var listmaxValueV3 = new List<int> { 1, 2, 3, 4, 5, 6, 76, 87, 9, 10, 0, 98, 23, 5, 12, 3453, 34, 56, 6, 23, 1, 34, 89 };
            var inputMaxList3 = ParallelAlgorithms.Map(listmaxValueV3, e => new List<int> { e });
            var maxValue3 = ParallelAlgorithms.Reduce(0, inputMaxList3.Length, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },/*参数e 为集合索引*/ e => listmaxValueV3[e], 0, (e, e1) => { return Math.Max(e1, e); });

            Assert.AreEqual(3453, maxValue3);
        }
      [Test]
        public void Test_ParallelAlgorithms_MapReduce_Array_Repeat()
        {
            //MR 求数组中的重复值
            var listmaxValueV3 = new List<int> { 1, 2, 3, 4, 5, 6, 76, 87, 9, 10, 0, 98, 23, 5, 12, 3453, 34, 56, 6, 23, 1, 34, 89 };
            var RepeatValue = listmaxValueV3.AsParallel().MapReduce(e => new List<int>() { e }, e => e, e =>
            {
                return new[] { new KeyValuePair<int, int>(e.Key, e.Count()) };
            }).Where(e => e.Value > 1).Select(e => e.Key);

            Assert.AreEqual(1, RepeatValue.FirstOrDefault());
        }
        #endregion
        #region   [ ParallelAlgorithms->ParallelAlgorithms_Scan & ScanInPlace ]
        [Test]
        public void Test_ParallelAlgorithms_Scan_Task_Scan()
        {
            var list = "a bb a bb cc Beim ersten Aufruf von FailMethod von FailMethod".Split(new char[] { ' ' }).ToList();
            //前缀扫描集合中元素
            var result = ParallelAlgorithms.Scan(list, (e1, e2) => e1 + e2);

            Assert.AreEqual("a", result[0]);
            Assert.AreEqual("abb", result[1]);
        }
        [Test]
        public void Test_ParallelAlgorithms_Scan_Task_ScanInPlace()
        {
            var list = "a bb a bb cc Beim ersten Aufruf von FailMethod von FailMethod".Split(new char[] { ' ' }).ToList();
            ParallelAlgorithms.ScanInPlace(list.ToArray(), (e1, e2) => e1 + e2);

            Assert.IsTrue(true);
        }
        #endregion
        #region   [ ParallelAlgorithms->ParallelAlgorithms_Sort ]
        [Test]
        public void Test_ParallelAlgorithms_Sort_ArraySort()
        {
            var list = new int[] { 100, 4, 5, 56, 23, 2, 1, 0, -99, 456, 234, 11, 9999, 44, 2 };
            ParallelAlgorithms.Sort(list);

            Assert.AreEqual(-99, list[0]);
            Assert.AreEqual(9999, list[list.Length - 1]);
        }
        #endregion
        #region [ ParallelAlgorithms->ParallelAlgorithms_SpeculativeFor & SpeculativeForEach ]
        [Test]
        public void Test_ParallelAlgorithms_SpeculativeFor_ArrayFor()
        {
            //内部执行并行循环，cas 
            var result = ParallelAlgorithms.SpeculativeFor(0, 5, e => e);
        }
        [Test]
        public void Test_ParallelAlgorithms_SpeculativeFor_ArrayForEach()
        {
            var list = new int[] { 100, 4, 5, 56, 23, 2, 1, 0, -99, 456, 234, 11, 9999, 44, 2 };
            //内部执行并行循环，cas ,替换掉数组内某个位置的值
            var result = ParallelAlgorithms.SpeculativeForEach(list, e => e);

            Assert.IsTrue(true);
        }
        #endregion
        #region [ ParallelAlgorithms->ParallelAlgorithms_SpeculativeInvoke ]
        [Test]
        public void Test_ParallelAlgorithms_SpeculativeInvoke_Invoke()
        {
            //并行执行两个任务，其中一个任务完成 就返回被执行的那个委托值
            var result = ParallelAlgorithms.SpeculativeInvoke(() => 1, () => { Thread.Sleep(2000); return 3; });
            Assert.AreEqual(1, result);
        }
        #endregion
        #region [ ParallelAlgorithms->ParallelAlgorithms_WhileNotEmpty ]
        public void Test_ParallelAlgorithms_WhileNotEmpty_While()
        {
            var list = new int[] { 100, 4, 5, 56, 23, 2, 1, 0, -99, 456, 234, 11, 9999, 44, 2 };
            //待研究
            ParallelAlgorithms.WhileNotEmpty(list, (e1, e2) => e2(0));

            Assert.IsTrue(true);
        }
        #endregion
        #region   [ CoordinationDataStructures->AsyncCoordination->AsyncCache ]
        [Test]
        public void Test_CoordinationDataStructures_AsyncCoordination_AsyncCache_Cache()
        {
            //异步cached
            var asyncCache = new AsyncCache<int, int>(e => Task.Factory.StartNew<int>(() => 10));
            var task = asyncCache.GetValue(1);
            Assert.AreEqual(true, task.IsCompleted);
            Assert.AreEqual(1, task.Result);

            var httpCache = new HtmlAsyncCache();
            var pageTask = httpCache.GetValue(new Uri("http://www.baidu.com"));
            Assert.AreEqual(true, pageTask.IsCompleted);
        }
        #endregion
        #region   [ CoordinationDataStructures->AsyncCoordination->AsyncCall ]
        [Test]
        public void Test_CoordinationDataStructures_AsyncCoordination_AsyncCall_1()
        {
            //
            //模式：为每个提交数据项异步调用一个处理程序
            //     生产、消费分两种模式，1、同步式：是消费者同步消费生产者的数据，等待生产者的数据，BlockingCollection<T>是属于这种模式  ；
            //                       2、异步式：生产者把数据加入到共享队列中，并通知监听此队列的消费者，从而消费者取出数据进行处理，“取出”数据
            //                          有两种模式，a，顺序取出；b，并行取出
            //
            var call = new AsyncCall<int>(
                 e =>
                 {
                     // 返回 task 为异步执行任务 ,直接使用action 为同步执行
                     var tt = new Task(() => Console.WriteLine(e));
                     tt.Start();
                     return tt;
                 }
                 , maxDegreeOfParallelism: Environment.ProcessorCount);
            call.Post(1);
            call.Post(2);
            call.Post(3);
            call.Post(4);

            Assert.IsTrue(true);
        }
      [Test]
        public void Test_CoordinationDataStructures_AsyncCoordination_AsyncCall_2()
        {
            //
            //模式：为每个提交数据项异步调用一个处理程序
            //

            //call1 生产者
            AsyncCall<double> call1 = null;
            AsyncCall<string> call2 = null;
            AsyncCall<int> call3 = null;

            call1 = new AsyncCall<double>(d => call2.Post(d.ToString()));
            call2 = new AsyncCall<string>(s => call3.Post(s.Length));
            call3 = new AsyncCall<int>(i => Console.WriteLine(i));


            call1.Post(0);
            call1.Post(2.2);
            call1.Post(3.3);
            call1.Post(4.4);

            Assert.IsTrue(true);
        }
        #endregion
        #region   [ CoordinationDataStructures->Pipeline ]
        [Test]
        public void Test_CoordinationDataStructures_Pipeline_Test()
        {
            //模式：流水线
            //流水线   方法 Create,参数 func 申明一个每次都会调用的函数 ,degreeOfParallelism指定并行深度
            var pipeLine = Pipeline
                .Create<string, int>(e => Convert.ToInt32(e), degreeOfParallelism: Environment.ProcessorCount)
                .Next(e => e);

            var result = pipeLine.Process(new List<string> { "1", "2", "3", "4", "5" });

            Assert.AreEqual(5, result.Count());
            Assert.AreEqual(1, result.ToArray()[0]);
        }
        #endregion
        #region   [ CoordinationDataStructures->ObjectPool ]
        [Test]
        public void Test_CoordinationDataStructures_ObjectPool()
        {
            //模式：对象池
            //对象池  构造函数new A_Pool 为初始化一个对象 当获取不到对象时使用这个这个默认的对象
            //这个策略比较简单，还可以再扩展下
            var objPool = new ObjectPool<A_Pool>(() => new A_Pool());
            //添加对象到池里
            for (var i = 0; i < 10; i++)
                objPool.PutObject(new A_Pool());
            //从池里面获取一个对象
            var apool = objPool.GetObject();
            Assert.AreEqual(9, objPool.Count);
            objPool.PutObject(apool);
            Assert.AreEqual(10, objPool.Count);
        }
        #endregion
        #region   [ CoordinationDataStructures->ConcurrentPriorityQueue ]
        [Test]
        public void Test_CoordinationDataStructures_ConcurrentPriorityQueue()
        {
            //模式：优先队列
            //实现原理，最小堆
            var pQ = new ConcurrentPriorityQueue<int, string>();
            pQ.Enqueue(0, "a0");
            pQ.Enqueue(1, "a1");
            pQ.Enqueue(2, "a2");
            pQ.Enqueue(3, "a3");
            pQ.Enqueue(4, "a4");
            pQ.Enqueue(-1, "-a1");

            KeyValuePair<int, string> kv;
            pQ.TryDequeue(out kv);

            Assert.AreEqual("-a1", kv.Value);
        }
        #endregion
        #region   [ CoordinationDataStructures->SerialTaskQueue ]
        [Test]
        public void Test_CoordinationDataStructures_SerialTaskQueue()
        {
            //模式：放入队列中的任务串联执行
            var q = new SerialTaskQueue();
            q.Enqueue(() => Task.Factory.StartNew(() => "1"));
            q.Enqueue(() => Task.Factory.StartNew(() => "2"));

            var isCompleted = q.Completed().IsCompleted;

            Assert.IsTrue(isCompleted);
        }
        #endregion
        #region   [ CoordinationDataStructures->SpinLockClass ]
        [Test]
        public void Test_CoordinationDataStructures_SpinLockClass()
        {
            //模式：对象持有锁执行指定的委托
            var spin = new SpinLockClass(true);
            spin.Execute(() => Console.WriteLine("1"));

            Assert.IsTrue(true);
        }
        #endregion
    }
}
