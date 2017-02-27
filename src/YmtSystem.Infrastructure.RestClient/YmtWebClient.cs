using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Web;
using System.Threading;
namespace YmtSystem.Infrastructure.RestClient
{
    public class YmtRestWebClient
    {
        private static readonly YmtWebClientPool pool = new YmtWebClientPool(() => new YmtRestWebClient());
        //base uri
        private const string baseAddress = "/json/reply";
        //test url
        private string baseUri = "http://192.168.1.247:2000";
        //local url
        //private string baseUri = "http://ymt.auth.com";

        public static YmtRestWebClient Instance { get { return pool.GetService(); } }
        public static void Clear() { pool.Dispose(); }

        #region
        //[ThreadStatic]
        //private static YmtAuthWebClient _instance;
        //public static YmtAuthWebClient Instance { get { return _instance == null ? (_instance = new YmtAuthWebClient()) : _instance; } }
        #endregion
        public TReturn Get<TReturn>(string path, IDictionary<string, string> query, char queryParametersSplitChar = '&', TReturn defReturn = default(TReturn), Action<Exception> errorHandle = null, int retry = 1, string requestUri = null, Dictionary<HttpRequestHeader, string> headerVal = null)
        {
            Func<TReturn> get = () =>
            {
                var _queryString = DicToQueryString(query, queryParametersSplitChar);
                var _tmpAddress = string.Format("{0}?{1}", path, _queryString);
                var _response = PrivateGet(requestUri ?? this.baseUri, _tmpAddress, headerVal);
                if (typeof(TReturn) == typeof(string)) return (TReturn)((object)_response);
                return JsonConvert.DeserializeObject<TReturn>(_response);
            };
            return TryExecute<TReturn>(get, defReturn, retry, errorHandle);
        }

        public TReturn Get<TReturn, TRequest>(TRequest requestDto, TReturn defReturn = default(TReturn), Action<Exception> errorHandle = null, int retry = 1, string requestUri = null)
        {
            Func<TReturn> get = () =>
            {
                var _queryString = DtoToQueryString<TRequest>(requestDto);
                //_queryString = HttpUtility.UrlEncode(_queryString, Encoding.GetEncoding("utf-8"));
                var _tmpAddress = string.Format("{0}/{1}?{2}", baseAddress, typeof(TRequest).Name, _queryString);

                var _response = PrivateGet(requestUri ?? this.baseUri, _tmpAddress);
                return JsonConvert.DeserializeObject<TReturn>(_response);
            };
            var result = TryExecute<TReturn>(get, defReturn, retry, errorHandle);
            return result;
        }
        public TReturn Post<TReturn, TRequest>(TRequest requestDto, TReturn defReturn = default(TReturn), Action<Exception> errorHandle = null, int retry = 0, string requestUri = null)
        {
            Func<TReturn> post = () =>
            {
                var _json = DtoToJson(requestDto);
                var _by = System.Text.Encoding.UTF8.GetBytes(_json);
                var _tmpAddress = string.Format("{0}/{1}", baseAddress, typeof(TRequest).Name);

                var _response = PrivatePost(requestUri ?? this.baseUri, _tmpAddress, _by);
                var _responseJson = System.Text.Encoding.UTF8.GetString(_response);

                return JsonConvert.DeserializeObject<TReturn>(_responseJson);
            };
            return TryExecute<TReturn>(post, defReturn, retry, errorHandle);
        }
        public void Post<TRequest>(TRequest requestDto, Action<Exception> errorHandle = null, int retry = 0, string requestUri = null)
        {
            Action action = () =>
            {
                var _json = DtoToJson(requestDto);
                var _by = System.Text.Encoding.UTF8.GetBytes(_json);
                var _tmpAddress = string.Format("{0}/{1}", baseAddress, typeof(TRequest).Name);

                var _response = PrivatePost(requestUri ?? this.baseUri, _tmpAddress, _by);
            };
            TryExecute(action, retry, errorHandle);
        }
        public void AsyncPost<TRequest>(TRequest requestDto, Action<Exception> errorHandle = null, int retry = 0, string requestUri = null)
        {
            Action action = () =>
            {
                var _json = DtoToJson(requestDto);
                var _by = System.Text.Encoding.UTF8.GetBytes(_json);
                var _tmpAddress = string.Format("{0}/{1}", baseAddress, typeof(TRequest).Name);

                PrivatePostAsync(requestUri ?? this.baseUri, _tmpAddress, _by);
            };

            TryExecute(action, retry, errorHandle);
        }
#if NET_4_5
        //public static int WebClientCount { get { return pool.InstanceCount; } }
#endif
        private Task<TResult> TryAsyncExecute<TResult>(Func<Task<TResult>> execute, Task<TResult> defReturnVal = default(Task<TResult>), int retry = 3, Action<Exception> errorHandler = null)
        {
            var success = true;
            var result = default(Task<TResult>);
            do
            {
                try
                {
                    success = true;
                    result = execute();
                }
                catch (Exception ex)
                {
                    success = false;
                    if (errorHandler != null) errorHandler(ex);
                }
            } while (!success && retry-- > 0);
            if (!success) return defReturnVal;
            return result;
        }
        private TResult TryExecute<TResult>(Func<TResult> execute, TResult defReturnVal = default(TResult), int retry = 3, Action<Exception> errorHandler = null)
        {
            var success = true;
            var result = default(TResult);
            do
            {
                try
                {
                    success = true;
                    result = execute();
                }
                catch (Exception ex)
                {
                    success = false;
                    if (errorHandler != null) errorHandler(ex);
                }
            } while (!success && retry-- > 0);
            if (!success) return defReturnVal;
            return result;
        }
        private void TryExecute(Action execute, int retry = 3, Action<Exception> errorHandler = null)
        {
            var success = true;
            do
            {
                try
                {
                    success = true;
                    execute();
                }
                catch (Exception ex)
                {
                    success = false;
                    if (errorHandler != null) errorHandler(ex);
                }
            } while (!success && retry-- > 0);
        }
        private string PrivateGet(string baseUri, string address, Dictionary<HttpRequestHeader, string> handlerVal = null)
        {
            var _web = new WebClientEx();
            _web.BaseAddress = baseUri;
            SetRequestHandler(handlerVal, _web);
            return _web.DownloadString(address);
        }
        private byte[] PrivatePost(string baseUri, string address, byte[] by, Dictionary<HttpRequestHeader, string> handlerVal = null)
        {
            var _web = new WebClientEx();
            _web.BaseAddress = baseUri;
            SetRequestHandler(handlerVal, _web);
            return _web.UploadData(address, by);
        }
        private void PrivatePostAsync(string baseUri, string address, byte[] by, Dictionary<HttpRequestHeader, string> handlerVal = null)
        {
            var _web = new WebClientEx();
            _web.BaseAddress = baseUri;
            SetRequestHandler(handlerVal, _web);
            _web.UploadDataAsync(new Uri(baseUri + address), "POST", by);
        }
        private void SetRequestHandler(Dictionary<HttpRequestHeader, string> handlerVal, WebClient webClient)
        {
            webClient.Encoding = Encoding.GetEncoding("utf-8");
            webClient.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
            webClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "utf-8");
            if (handlerVal != null)
            {
                foreach (var item in handlerVal)
                    webClient.Headers.Add(item.Key, item.Value);
            }
            else
            {
                webClient.Headers.Add(HttpRequestHeader.Accept, "application/json");
                webClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "ServiceStack .NET Client 4.00");
            }
        }
        private static string DtoToQueryString<TDto>(TDto requestDto)
        {
            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                };
            };
            var json = JsonConvert.SerializeObject(requestDto);
            var _queryString = json
                                    .Replace("{", "")
                                    .Replace("}", "")
                                    .Replace(":", "=")
                                    .Replace(",", "&")
                                    .Replace("\"", "");
            return _queryString;
        }
        private static string DicToQueryString(IDictionary<string, string> dicSource, char queryParametersSplitChar = '&')
        {
            if (!dicSource.Any()) return string.Empty;
            var str = new StringBuilder();
            foreach (var item in dicSource)
            {
                str.AppendFormat("{0}={1}{2}", item.Key, item.Value, queryParametersSplitChar);
            }
            return str.ToString().TrimEnd(queryParametersSplitChar);
        }
        private static string DtoToJson<TDto>(TDto requestDto)
        {
            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                };
            };
            var json = JsonConvert.SerializeObject(requestDto);
            return json;
        }
        private static void AssertAuthUriNotNull(string val)
        {
            if (string.IsNullOrEmpty(val))
                throw new InvalidOperationException("请配置认证host");
        }
        private YmtRestWebClient()
        {
            this.baseUri = System.Configuration.ConfigurationManager.AppSettings.Get("UserAuthServiceHost");
            this.baseUri = string.IsNullOrEmpty(this.baseUri) ? "http://192.168.1.247:2000" : this.baseUri;
            AssertAuthUriNotNull(this.baseUri);
        }

    }
    internal class YmtWebClientPool : IDisposable
    {
        private Func<YmtRestWebClient> _client;
#if NET_4_5
        private static readonly ThreadLocal<YmtAuthWebClient> th = new ThreadLocal<YmtAuthWebClient>(true);
#else
        private static readonly ThreadLocal<YmtRestWebClient> th = new ThreadLocal<YmtRestWebClient>();
#endif
        public YmtWebClientPool(Func<YmtRestWebClient> _client)
        {
            this._client = _client;
        }

        public YmtRestWebClient GetService()
        {
            if (th.Value == null)
                this.AddToThreadLocal(_client());
            return th.Value;
        }
#if NET_4_5
        public int InstanceCount
        {
            get { return IsWeb ? WebClientCount : th.Values.Count; }
        }
#endif
        #region
        //private int WebClientCount
        //{
        //    get { try { return HttpContext.Current.Items.OfType<YmtAuthWebClient>().Count(); } catch { return 0; } }
        //}
        //private bool IsWeb
        //{
        //    get { return HttpContext.Current != null; }
        //}
        //private void AddToHttpCountext(YmtAuthWebClient client)
        //{
        //    HttpContext.Current.Items.Add(this.key, client);
        //}
        #endregion
        private void AddToThreadLocal(YmtRestWebClient client)
        {
            th.Value = client;
        }
        public void Dispose()
        {
            try
            {
#if NET_4_5
                th.Values.Clear();
#endif
                th.Dispose();
            }
            catch
            { }
        }
    }
    internal class WebClientEx : WebClient
    {
        private int _timeOut;
        public WebClientEx()
        {
            this._timeOut = 30000;
        }
        public WebClientEx(int timeOut = 30000)
        {
            this._timeOut = timeOut;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var r = (HttpWebRequest)base.GetWebRequest(address);
            r.Timeout = this._timeOut;
            r.ServicePoint.UseNagleAlgorithm = false;
            r.ServicePoint.ConnectionLimit = 1024;
            return r;
        }
    }
}
