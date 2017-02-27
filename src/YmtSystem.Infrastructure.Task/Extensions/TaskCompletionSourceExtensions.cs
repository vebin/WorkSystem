//--------------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved. 
// 
//  File: TaskCompletionSourceExtensions.cs
//
//--------------------------------------------------------------------------

namespace System.Threading.Tasks
{
    /// <summary>Extension methods for TaskCompletionSource.</summary>
    public static class TaskCompletionSourceExtensions
    {
        /// <summary>Transfers the result of a Task to the TaskCompletionSource.</summary>
        /// <typeparam name="TResult">Specifies the type of the result.</typeparam>
        /// <param name="resultSetter">The TaskCompletionSource.</param>
        /// <param name="task">The task whose completion results should be transfered.</param>
        public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion: resultSetter.SetResult(task is Task<TResult> ? ((Task<TResult>)task).Result : default(TResult)); break;
                case TaskStatus.Faulted: resultSetter.SetException(task.Exception.InnerExceptions); break;
                case TaskStatus.Canceled: resultSetter.SetCanceled(); break;
                default: throw new InvalidOperationException("The task was not completed.");
            }
        }

        /// <summary>Transfers the result of a Task to the TaskCompletionSource.</summary>
        /// <typeparam name="TResult">Specifies the type of the result.</typeparam>
        /// <param name="resultSetter">The TaskCompletionSource.</param>
        /// <param name="task">The task whose completion results should be transfered.</param>
        public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task<TResult> task)
        {
            SetFromTask(resultSetter, (Task)task);
        }

        /// <summary>Attempts to transfer the result of a Task to the TaskCompletionSource.</summary>
        /// <typeparam name="TResult">Specifies the type of the result.</typeparam>
        /// <param name="resultSetter">The TaskCompletionSource.</param>
        /// <param name="task">The task whose completion results should be transfered.</param>
        /// <returns>Whether the transfer could be completed.</returns>
        public static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion: return resultSetter.TrySetResult(task is Task<TResult> ? ((Task<TResult>)task).Result : default(TResult));
                case TaskStatus.Faulted: return resultSetter.TrySetException(task.Exception.InnerExceptions);
                case TaskStatus.Canceled: return resultSetter.TrySetCanceled();
                default: throw new InvalidOperationException("The task was not completed.");
            }
        }

        /// <summary>Attempts to transfer the result of a Task to the TaskCompletionSource.</summary>
        /// <typeparam name="TResult">Specifies the type of the result.</typeparam>
        /// <param name="resultSetter">The TaskCompletionSource.</param>
        /// <param name="task">The task whose completion results should be transfered.</param>
        /// <returns>Whether the transfer could be completed.</returns>
        public static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task<TResult> task)
        {
            return TrySetFromTask(resultSetter, (Task)task);
        }
    }

    public static class TimerTaskFactory
    {
        private static readonly TimeSpan DoNotRepeat = TimeSpan.FromMilliseconds(-1);

        /// <summary>
        /// Starts a new task that will poll for a result using the specified function, and will be completed when it satisfied the specified condition.
        /// </summary>
        /// <typeparam name="T">The type of value that will be returned when the task completes.</typeparam>
        /// <param name="getResult">Function that will be used for polling.</param>
        /// <param name="isResultValid">Predicate that determines if the result is valid, or if it should continue polling</param>
        /// <param name="pollInterval">Polling interval.</param>
        /// <param name="timeout">The timeout interval.</param>
        /// <returns>The result returned by the specified function, or <see langword="null"/> if the result is not valid and the task times out.</returns>
        public static Task<T> StartNew<T>(Func<T> getResult, Func<T, bool> isResultValid, TimeSpan pollInterval, TimeSpan timeout)
        {
            Timer timer = null;
            TaskCompletionSource<T> taskCompletionSource = null;
            DateTime expirationTime = DateTime.UtcNow.Add(timeout);

            timer =
                new Timer(_ =>
                {
                    try
                    {
                        if (DateTime.UtcNow > expirationTime)
                        {
                            timer.Dispose();
                            taskCompletionSource.SetResult(default(T));
                            return;
                        }

                        var result = getResult();

                        if (isResultValid(result))
                        {
                            timer.Dispose();
                            taskCompletionSource.SetResult(result);
                        }
                        else
                        {
                            // try again
                            timer.Change(pollInterval, DoNotRepeat);
                        }
                    }
                    catch (Exception e)
                    {
                        timer.Dispose();
                        taskCompletionSource.SetException(e);
                    }
                });

            taskCompletionSource = new TaskCompletionSource<T>(timer);

            timer.Change(pollInterval, DoNotRepeat);

            return taskCompletionSource.Task;
        }
    }
}
