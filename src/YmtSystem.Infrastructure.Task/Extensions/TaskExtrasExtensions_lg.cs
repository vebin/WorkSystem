namespace System.Threading.Tasks
{  
    public static class TaskExtrasExtensions_lg
    {
        /// <summary>
        /// 关联到父级任务确保父级任务完成并返回当前任务task
        /// </summary>
        /// <typeparam name="Result"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        /// modify by lg 2013.7.17
        public static Task<TResult> AttachToParentAndReturnTask<TResult>(this Task<TResult> task)
        {
            if (task == null) throw new ArgumentNullException("task");

            var result = new TaskCompletionSource<TResult>();
            result.TrySetFromTask(task);

            task.ContinueWith(t => t.Wait()
            , CancellationToken.None
            , TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.ExecuteSynchronously
            , TaskScheduler.Default);

            return result.Task;
        }
    }
}
