namespace YmtSystem.CrossCutting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class ExecuteResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ExecuteResult(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            ResultData = data;
        }

        public static ExecuteResult<T> IsSuccess(T data)
        {
            return new ExecuteResult<T>(true, string.Empty, data);
        }

        public static ExecuteResult<T> IsSuccess(T data, string message)
        {
            return new ExecuteResult<T>(true, message, data);
        }

        public static ExecuteResult<T> IsFail(string message)
        {
            return new ExecuteResult<T>(false, message, default(T));
        }

        public static ExecuteResult<T> IsFail(T data, string message)
        {
            return new ExecuteResult<T>(false, message, data);
        }
        
       
        /// <summary>
        /// 是否成功
        /// </summary>
        //[DataMember]
        public bool Success { get; set; }
       
        /// <summary>
        /// 返回消息
        /// </summary>
        //[DataMember]
        public string Message { get; set; }       
        /// <summary>
        /// 返回数据
        /// </summary>
        //[DataMember]
        public T ResultData { get; set; }

        public static ExecuteResult<T> CreateSuccess(T successdata, string message)
        {
            return new ExecuteResult<T>(true, message, successdata);
        }

        public static ExecuteResult<T> CreateFaile(T faileddata, string message)
        {
            return new ExecuteResult<T>(false, message, faileddata);
        }
    }
}
