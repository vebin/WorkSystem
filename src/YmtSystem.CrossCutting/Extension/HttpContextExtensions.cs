namespace System.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    public static class HttpContextExtensions
    {
        public static TResult RequestAs<TResult>(this HttpRequest request, string key, TResult defVal = default(TResult))
        {
            if (request == null
                || string.IsNullOrEmpty(key)
                || request.QueryString == null
                || string.IsNullOrEmpty(request[key])) return defVal;

            return request[key].ConvertTo<TResult>(defVal);
        }

        public static Dictionary<object, string> QueryStringToDictionary(this HttpRequestBase request)
        {
            if (request == null || request.QueryString == null) return new Dictionary<object, string>();
            var queryDic = new Dictionary<object, string>();
            foreach (var item in request.QueryString.Keys)
            {
                queryDic[item] = request.QueryString[item.ToString()];
            }
            return queryDic;
        }

        public static Dictionary<object, string> QueryStringToDictionary(this HttpRequest request)
        {
            if (request == null || request.QueryString == null) return new Dictionary<object, string>();
            var queryDic = new Dictionary<object, string>();
            foreach (var item in request.QueryString.Keys)
            {
                queryDic[item] = request.QueryString[item.ToString()];
            }
            return queryDic;
        }

        public static void SetCookie(this HttpResponse response, string key, string value, string domain = null, bool httpOnly = false, DateTime? expires = null)
        {
            if (response == null) return;
            var cookie = new HttpCookie(key, value);
            cookie.HttpOnly = httpOnly;
            if (expires.HasValue)
                cookie.Expires = expires.Value;
            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;
            response.SetCookie(cookie);
        }
        public static void SetCookie(this HttpResponseBase response, string key, string value, string domain = null, bool httpOnly = false, DateTime? expires = null)
        {
            if (response == null) return;
            var cookie = new HttpCookie(key, value);
            cookie.HttpOnly = httpOnly;
            if (expires.HasValue)
                cookie.Expires = expires.Value;
            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;
            response.SetCookie(cookie);
        }
        public static void AppendCookie(this HttpResponse response, string key, string value, string domain = null, bool httpOnly = false, DateTime? expires = null)
        {
            if (response == null) return;
            var cookie = new HttpCookie(key, value);
            cookie.HttpOnly = httpOnly;
            if (expires.HasValue)
                cookie.Expires = expires.Value;
            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;
            response.AppendCookie(cookie);
        }
        public static void AppendCookie(this HttpResponseBase response, string key, string value, string domain = null, bool httpOnly = false, DateTime? expires = null)
        {
            if (response == null) return;
            var cookie = new HttpCookie(key, value);
            cookie.HttpOnly = httpOnly;
            if (expires.HasValue)
                cookie.Expires = expires.Value;
            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;
            response.AppendCookie(cookie);
        }
        public static void AddCookie(this HttpResponse response, string key, string value, string domain = null, bool httpOnly = false, DateTime? expires = null)
        {
            if (response == null) return;
            var cookie = new HttpCookie(key, value);
            cookie.HttpOnly = httpOnly;
            if (expires.HasValue)
                cookie.Expires = expires.Value;
            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;
            response.Cookies.Add(cookie);
        }
        public static void AddCookie(this HttpResponseBase response, string key, string value, string domain = null, bool httpOnly = false, DateTime? expires = null)
        {
            if (response == null) return;
            var cookie = new HttpCookie(key, value);
            cookie.HttpOnly = httpOnly;
            if (expires.HasValue)
                cookie.Expires = expires.Value;
            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;
            response.Cookies.Add(cookie);
        }
        public static string ReadCookie(this HttpRequest request, string key, string defVal = null)
        {
            if (request == null
                || request.Cookies == null
                || request.Cookies[key] == null
                || request.Cookies[key].Value == null) return defVal;
            return request.Cookies[key].Value;
        }
        public static string ReadCookie(this HttpRequestBase request, string key, string defVal = null)
        {
            if (request == null
                || request.Cookies == null
                || request.Cookies[key] == null
                || request.Cookies[key].Value == null) return defVal;
            return request.Cookies[key].Value;
        }
        public static TResult ReadCookie<TResult>(this HttpRequest request, string key, TResult defVal = default(TResult))
        {
            if (request == null
                || request.Cookies == null
                || request.Cookies[key] == null
                || request.Cookies[key].Value == null) return defVal;
            return request.Cookies[key].Value.ConvertTo(defVal);
        }
        public static TResult ReadCookie<TResult>(this HttpRequestBase request, string key, TResult defVal = default(TResult))
        {
            if (request == null
                || request.Cookies == null
                || request.Cookies[key] == null
                || request.Cookies[key].Value == null) return defVal;
            return request.Cookies[key].Value.ConvertTo(defVal);
        }
        public static void RemoveCookie(this HttpResponse response, string key, string doMain = null)
        {
            var cookie = response.Cookies[key];
            if (cookie == null)
                return;
            response.Cookies.Remove(cookie.Name);
            if (!string.IsNullOrEmpty(doMain))
                cookie.Domain = doMain;
            cookie.Value = null;
            cookie.Expires = DateTime.Now.AddYears(-1);

            response.Cookies.Add(cookie);
        }
        public static void RemoveCookie(this HttpResponseBase response, string key, string doMain = null)
        {
            var cookie = response.Cookies[key];
            if (cookie == null)
                return;
            response.Cookies.Remove(cookie.Name);
            if (!string.IsNullOrEmpty(doMain))
                cookie.Domain = doMain;
            cookie.Value = null;
            cookie.Expires = DateTime.Now.AddYears(-1);

            response.Cookies.Add(cookie);
        }
        [Obsolete("不要使用")]
        public static bool CheckCookiesIsExpires(this HttpRequest request, string key)
        {
            return true;
        }
        public static void JsonpResponse(this HttpResponse response, string jsonCallbackName, Func<string> message, HttpStatusCode statusCode = HttpStatusCode.OK, bool responseEnd = true)
        {
            if (string.IsNullOrEmpty(jsonCallbackName)) return;
            var responseMsg = message();
            response.Write(string.Format("{0}({1}{3}{2})", jsonCallbackName, "{", "}", responseMsg));
            response.StatusCode = (int)statusCode;
            if (responseEnd)
                response.End();
        }
        public static string RemoveQueryStringKey(this string uriString, string key, Action<Exception> errorHandle = null)
        {
            var _return = uriString;
            try
            {
                var uri = new Uri(uriString);
                var queryCoollection = HttpUtility.ParseQueryString(uri.Query);
                if (queryCoollection != null && queryCoollection.Count > 0 && queryCoollection.AllKeys.Contains(key))
                {
                    queryCoollection.Remove(key);
                    var query = queryCoollection.ToString();
                    if (uri.Port != 80)
                        return string.Format("{0}://{1}:{2}{3}?{4}", uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, query);
                    if (!string.IsNullOrEmpty(query))
                        return string.Format("{0}://{1}{2}?{3}", uri.Scheme, uri.Host, uri.AbsolutePath, query);
                    else
                        return string.Format("{0}://{1}{2}", uri.Scheme, uri.Host, uri.AbsolutePath);
                }
                return _return;
            }
            catch (Exception ex)
            {
                if (errorHandle != null)
                    errorHandle(ex);
                return _return;
            }
        }
        public static string SetQueryString(this string uriString, string key, string value, bool urlEncode = true, Action<Exception> errorHandle = null)
        {
            try
            {
                var uri = new Uri(uriString);
                var _tmpVal = urlEncode ? HttpUtility.UrlEncode(value) : value;
                if (string.IsNullOrEmpty(uri.Query))
                {
                    return string.Format("{0}?{1}={2}", uriString, key, _tmpVal);
                }
                else
                {
                    var queryCoollection = HttpUtility.ParseQueryString(uri.Query);
                    if (queryCoollection.AllKeys.Contains(key))
                    {
                        queryCoollection.Set(key, _tmpVal);
                        var query = queryCoollection.ToString();
                        if (uri.Port != 80)
                            return string.Format("{0}://{1}:{2}{3}?{4}", uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, query);
                        return string.Format("{0}://{1}{2}?{3}", uri.Scheme, uri.Host, uri.AbsolutePath, query);
                    }
                    else
                    {
                        queryCoollection.Add(key, _tmpVal);
                        if (uri.Port != 80)
                            return string.Format("{0}://{1}:{2}{3}?{4}", uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, queryCoollection.ToString());
                        return string.Format("{0}://{1}{2}?{3}", uri.Scheme, uri.Host, uri.AbsolutePath, queryCoollection.ToString());
                        //return string.Format("{0}&{1}={2}", uriString, key, _tmpVal);
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorHandle != null) errorHandle(ex);
                return uriString;
            }
        }
    }
}
