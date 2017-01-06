/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
//#define __UT_TEST_0EC173788C65DD08DA60575219707632__
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using Aliyun.Api.LOG;
using Aliyun.Api.LOG.Common.Utilities;
using Aliyun.Api.LOG.Utilities;
using System.Threading;
namespace Aliyun.Api.LOG.Common.Communication
{
    /// <summary>
    /// An default <see cref="ServiceClient"/> implementation that
    /// communicates with Aliyun Services over the HTTP protocol.
    /// </summary>
    internal class ServiceClientImpl : ServiceClient
    {

        #region Embeded Classes

        /// <summary>
        /// Represents the async operation of requests in <see cref="ServiceClientImpl"/>.
        /// </summary>
        private class HttpAsyncResult : AsyncResult<ServiceResponse>
        {
            public HttpWebRequest WebRequest { get; set; }
            
            public ExecutionContext Context { get; set; }

            public HttpAsyncResult(AsyncCallback callback, object state)
                : base(callback, state)
            {
            }
        }

        /// <summary>
        /// Represents the response data of <see cref="ServiceClientImpl"/> requests.
        /// </summary>
        internal class ResponseImpl : ServiceResponse
        {
            private bool _disposed;
            private HttpWebResponse _response;
            private Exception _failure;
#if(__UT_TEST_0EC173788C65DD08DA60575219707632__)
            private HttpStatusCode _httpStatusCode = HttpStatusCode.OK;
#endif
            public override HttpStatusCode StatusCode
            {
                get
                {
#if(__UT_TEST_0EC173788C65DD08DA60575219707632__)
                    return _httpStatusCode;
#else
                    return _response.StatusCode;
#endif
                }
#if(__UT_TEST_0EC173788C65DD08DA60575219707632__)
                set {
                    _httpStatusCode = value;
                }
#endif
            }
            
            public override Exception Failure {
                get
                {
                    return this._failure;
                }
            }

            public override IDictionary<string, string> Headers
            {
                get
                {
                    ThrowIfObjectDisposed();
                    if (_headers == null)
                    {
                        _headers = GetResponseHeaders(_response);
                    }

                    return _headers;
                }
                
            }
#if(__UT_TEST_0EC173788C65DD08DA60575219707632__)
            private Stream _stream;
#endif
            public override Stream Content
            {
                get
                {
                    ThrowIfObjectDisposed();

                    try
                    {
#if(__UT_TEST_0EC173788C65DD08DA60575219707632__)
                        return _response != null ?
                            _response.GetResponseStream() : _stream;
#else
                        return _response != null ?
                            _response.GetResponseStream() : null;
#endif
                    }
                    catch (ProtocolViolationException ex)
                    {
                        throw new InvalidOperationException(ex.Message, ex);
                    }
                }
#if(__UT_TEST_0EC173788C65DD08DA60575219707632__)
                set
                {
                    _stream = value;
                }
#endif
            }
            public ResponseImpl(HttpWebResponse httpWebResponse)
            {
                Debug.Assert(httpWebResponse != null);
                _response = httpWebResponse;

                Debug.Assert(this.IsSuccessful(), "This constructor only allows a successfull response.");
            }

            public ResponseImpl(WebException failure)
            {
                Debug.Assert(failure != null);
                HttpWebResponse httpWebResponse = failure.Response as HttpWebResponse;
                Debug.Assert(httpWebResponse != null);

                _failure = failure;
                _response = httpWebResponse;
                Debug.Assert(!this.IsSuccessful(), "This constructor only allows a failed response.");
            }
//#if(__UT_TEST_0EC173788C65DD08DA60575219707632__)
            internal ResponseImpl()
            {
            }
//#endif


            private static IDictionary<string, string> GetResponseHeaders(HttpWebResponse response)
            {
#if(__UT_TEST_0EC173788C65DD08DA60575219707632__)
                if (response == null)
                {
                    IDictionary<string, string> testHeaders = new Dictionary<String, String>();
                    testHeaders.Add(LogConsts.NAME_HEADER_AUTH, "LOG mockkeyid:EX2VSCpdyFrcysmBaQ+aokupwcg=");
                    return testHeaders;
                }
#endif
                var headers = response.Headers;
                var result = new Dictionary<string, string>(headers.Count,StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < headers.Count; i++)
                {
                    var key = headers.Keys[i];
                    var value = headers.Get(key);
                    result.Add(key,
                               HttpUtils.ReEncode(
                                   value,
                                   HttpUtils.Iso88591Charset,
                                   HttpUtils.UTF8Charset));
                }

                return result;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (_disposed)
                {
                    return;
                }

                if (disposing)
                {
                    if (_response != null)
                    {
                        _response.Close();
                        _response = null;
                    }
                    _disposed = true;
                }
            }

            private void ThrowIfObjectDisposed()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }
            }
        }

        #endregion

        #region Constructors

        public ServiceClientImpl(ClientConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Implementations
        internal delegate ServiceResponse WebSend(HttpWebRequest request);
        internal WebSend SendMethod = DefaultSend;
        internal static ResponseImpl DefaultSend(HttpWebRequest request)
        {
            var response = request.GetResponse() as HttpWebResponse;
            return new ResponseImpl(response);
        }
        private static ServiceResponse HandleException(WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response == null)
            {
                throw new LogException("LogRequestError", "request is failed.", ex);
            }
            else
            {
                return new ResponseImpl(ex);
            }
        }
        protected override ServiceResponse SendCore(ServiceRequest serviceRequest,
                                                    ExecutionContext context)
        {
            Random rnd = new Random();
            try
            {
                HttpWebRequest request = HttpFactory.CreateWebRequest(serviceRequest, Configuration);
#if(!__UT_TEST_0EC173788C65DD08DA60575219707632__)
                SetRequestContent(request, serviceRequest);
#endif
                return SendMethod(request);
            }
            catch (WebException ex)
            {
                return HandleException(ex);
            }
            
        }

        private static void SetRequestContent(HttpWebRequest webRequest,
                                              ServiceRequest serviceRequest)
        {

            var data = serviceRequest.BuildRequestContent();

            if (data == null ||
                (serviceRequest.Method != HttpMethod.Put &&
                 serviceRequest.Method != HttpMethod.Post))
            {
                // Skip setting content body in this case.
                webRequest.ContentLength = 0;
                return;
            }

            // Write data to the request stream.
            long userSetContentLength = -1;
            if (serviceRequest.Headers.ContainsKey(HttpHeaders.ContentLength))
            {
                userSetContentLength = long.Parse(serviceRequest.Headers[HttpHeaders.ContentLength]);
            }
            
            long streamLength =  data.Length - data.Position;
            webRequest.ContentLength = (userSetContentLength >= 0 && userSetContentLength <= streamLength) ?
            userSetContentLength : streamLength;

            //webRequest.
            //webRequest.KeepAlive = true;
            //webRequest.ReadWriteTimeout = 100000000;
            //webRequest.ServicePoint.ConnectionLimit = 100000;
            //webRequest.Timeout = Configuration
            using (var requestStream = webRequest.GetRequestStream())
            {
                data.WriteTo(requestStream, webRequest.ContentLength);
                data.Seek(0, SeekOrigin.Begin);
            }
        }

        #endregion

    }

    internal static class HttpFactory
    {
        internal static HttpWebRequest CreateWebRequest(ServiceRequest serviceRequest, ClientConfiguration configuration)
        {
            Debug.Assert(serviceRequest != null && configuration != null);

            HttpWebRequest webRequest = WebRequest.Create(serviceRequest.BuildRequestUri()) as HttpWebRequest;

            SetRequestHeaders(webRequest, serviceRequest, configuration);
            SetRequestProxy(webRequest, configuration);

            return webRequest;
        }

        // Set request headers
        private static void SetRequestHeaders(HttpWebRequest webRequest, ServiceRequest serviceRequest,
                                              ClientConfiguration configuration)
        {
            webRequest.Timeout = configuration.ConnectionTimeout;
            webRequest.ReadWriteTimeout = configuration.ReadWriteTimeout;
            webRequest.Method = serviceRequest.Method.ToString().ToUpperInvariant();

            // Because it is not allowed to set common headers
            // with the WebHeaderCollection.Add method,
            // we have to call an internal method to skip validation.
            foreach (var h in serviceRequest.Headers)
            {
                //if (h.Key.CompareTo(LogConsts.NAME_HEADER_HOST) == 0)
                //    webRequest.Host = h.Value;
                //else if (h.Key.CompareTo(LogConsts.NAME_HEADER_DATE) == 0)
                //    webRequest.Date = DateUtils.ParseRfc822Date(h.Value);
                //if (h.Key.CompareTo(LogConsts.NAME_HEADER_CONTENTTYPE) == 0)
                //    webRequest.ContentType = h.Value;
                //else
                webRequest.Headers.AddInternal(h.Key, h.Value);
            }
            
            // Set user-agent
            if (!string.IsNullOrEmpty(configuration.UserAgent))
            {
                webRequest.UserAgent = configuration.UserAgent;
            }
        }

        // Set proxy
        private static void SetRequestProxy(HttpWebRequest webRequest, ClientConfiguration configuration)
        {
            // Perf Improvement:
            // If HttpWebRequest.Proxy is not set to null explicitly,
            // it will try to load the IE proxy settings including auto proxy detection,
            // which is quite time consuming.
            webRequest.Proxy = null;

            // Set proxy if proxy settings are specified.
            if (!string.IsNullOrEmpty(configuration.ProxyHost))
            {
                if (configuration.ProxyPort < 0)
                {
                    webRequest.Proxy = new WebProxy(configuration.ProxyHost);
                } else
                {
                    webRequest.Proxy = new WebProxy(configuration.ProxyHost, configuration.ProxyPort);
                }

                if (!string.IsNullOrEmpty(configuration.ProxyUserName)) {
                    webRequest.Proxy.Credentials = String.IsNullOrEmpty(configuration.ProxyDomain) ?
                        new NetworkCredential(configuration.ProxyUserName, configuration.ProxyPassword ?? string.Empty) :
                        new NetworkCredential(configuration.ProxyUserName, configuration.ProxyPassword ?? string.Empty,
                                              configuration.ProxyDomain);
                }
            }
        }

    }

    internal static class HttpExtensions
    {
        private static MethodInfo _addInternalMethod;
        private static ICollection<PlatformID> monoPlatforms = new List<PlatformID>
        { PlatformID.MacOSX, PlatformID.Unix };
        private static bool? isMonoPlatform;

        internal static void AddInternal(this WebHeaderCollection headers, string key, string value)
        {
            if (isMonoPlatform == null)
            {
                isMonoPlatform = monoPlatforms.Contains(System.Environment.OSVersion.Platform);
            }
            
            // HTTP headers should be encoded to iso-8859-1,
            // however it will be encoded automatically by HttpWebRequest in mono.
            if (isMonoPlatform == false)
                // Encode headers for win platforms.
                value = HttpUtils.ReEncode(
                    value,
                    HttpUtils.UTF8Charset,
                    HttpUtils.Iso88591Charset);

            if (_addInternalMethod == null)
            {
                // Specify the internal method name for adding headers
                // mono: AddWithoutValidate
                // win: AddInternal
                string internalMethodName = (isMonoPlatform == true) ? "AddWithoutValidate" : "AddInternal";

                MethodInfo mi = typeof(WebHeaderCollection).GetMethod(
                    internalMethodName,
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new Type[] { typeof(string), typeof(string) },
                    null);
                _addInternalMethod = mi;
            }

            _addInternalMethod.Invoke(headers, new object[] { key, value });
        }
    }
}
