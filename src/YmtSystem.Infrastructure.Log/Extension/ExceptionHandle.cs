namespace Ymatou.Infrastructure.Log
{
    using System; 
    using System.Threading.Tasks;
    using Ymatou.CrossCutting;

    public static class ExceptionHandle
    {
        public static void Handle(this Exception ex, string msg = "", bool async = false)
        {
            //TODO:实现异常处理
            if (async)
            {
                Task.Factory.StartNew(() => 
                {
                    YmatouLog.Log.Error(msg, ex);
                });
            }
            else
            {
                YmatouLog.Log.Error(msg, ex);
            }
        }

        public static void Handle<T>(this Exception<T> ex, string msg = "", bool async = false) where T : ExceptionArgs
        {
            //TODO:实现异常处理
            if (async)
            {
                Task.Factory.StartNew(() =>
                {
                    YmatouLog.Log.Error(msg, ex);
                });
            }
            else
            {
                YmatouLog.Log.Error(msg, ex);
            }
        }
    }
}
