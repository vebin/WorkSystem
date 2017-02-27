namespace System.Threading.Tasks
{
    using System.Threading.Tasks;

    public static partial class TaskFactoryExtensions
    {
        /// <summary>
        ///  Ensure Task Completed
        /// </summary>
        /// <param name="task">task</param>
        /// <param name="timeOut">timeOut</param>
        public static Task<T> EnsureTaskCompleted<T>(this Task<T> task, TimeSpan timeOut = default(TimeSpan))
        {
            if (timeOut != default(TimeSpan))
                System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion, timeOut);
            System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion);

            return task;
        }
        /// <summary>
        ///  Ensure Task Completed
        /// </summary>
        /// <param name="task">task</param>
        /// <param name="timeOut">timeOut</param>
        public static Task EnsureTaskCompleted(this Task task, TimeSpan timeOut = default(TimeSpan))
        {
            if (timeOut != default(TimeSpan))
            {
                if (!System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion, timeOut))
                {

                }
            }
            System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion);

            return task;
        }

        /// <summary>
        ///  Ensure Task Completed
        /// </summary>
        /// <param name="task">task</param>
        /// <param name="timeOut">timeOut</param>
        public static Tuple<Task<T>, bool> ReturnTaskCompleted<T>(this Task<T> task, TimeSpan timeOut = default(TimeSpan), bool cancel = false)
        {
            var tmpTask = new TaskCompletionSource<T>(task.AsyncState);

            if (timeOut != default(TimeSpan))
            {
                if (!System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion, timeOut))
                {
                    if (cancel)
                    {
                        tmpTask.TrySetCanceled();
                    }
                    return Tuple.Create<Task<T>, bool>(tmpTask.Task, true);
                }
            }
            else
            {
                System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion);
            }
            return Tuple.Create<Task<T>, bool>(tmpTask.Task, false);

        }

        public static Tuple<Task<T>, bool> ReturnTaskCompleted<T>(this Task<T> task, TimeSpan timeOut = default(TimeSpan), CancellationTokenSource token = null)
        {
            var tmpTask = new TaskCompletionSource<T>(task.AsyncState);

            if (timeOut != default(TimeSpan))
            {
                if (!System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion, timeOut))
                {
                    if (token != null)
                    {
                        token.Cancel();
                        tmpTask.TrySetCanceled();
                    }
                    return Tuple.Create<Task<T>, bool>(tmpTask.Task, true);
                }
            }
            else
            {
                System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion);
            }
            return Tuple.Create<Task<T>, bool>(tmpTask.Task, false);

        }
        /// <summary>
        ///  Ensure Task Completed
        /// </summary>
        /// <param name="task">task</param>
        /// <param name="timeOut">timeOut</param>
        public static Tuple<Task, bool> ReturnTaskCompleted(this Task task, TimeSpan timeOut = default(TimeSpan), bool cancel = false)
        {
            var tmpTask = new TaskCompletionSource<object>(task.AsyncState);
            if (timeOut != default(TimeSpan))
            {
                if (!System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion, timeOut))
                {
                    if (cancel)
                        tmpTask.TrySetCanceled();
                    return Tuple.Create<Task, bool>(tmpTask.Task, true);
                }
            }
            else
                System.Threading.SpinWait.SpinUntil(() => task.Status == TaskStatus.RanToCompletion);

            return Tuple.Create<Task, bool>(tmpTask.Task, false);
        }
    }
}
