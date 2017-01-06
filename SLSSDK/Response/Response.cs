/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aliyun.Api.LOG.Utilities;

namespace Aliyun.Api.LOG.Response
{
    /// <summary>
    /// Super class of SLS response
    /// </summary>
    public class LogResponse
    {
        // Http header of the response
        private Dictionary<String, String> _headers = new Dictionary<String, String>();

        /// <summary>
        /// LogResponse constructor with HTTP response headers
        /// </summary>
        /// <param name="httpHeaders">HTTP response header from SLS server</param>        
        public LogResponse(IDictionary<String, String> httpHeaders)
        {
            _headers = new Dictionary<String, String>(httpHeaders);
        }

        /// <summary>
        /// Get the value from the head of response using key
        /// </summary>
        /// <returns>Value of specified http header</returns>
        public String GetHeader(String key)
        {
            String res = null;
            _headers.TryGetValue(key, out res);
            return res;
        }

        /// <summary>
        /// Get request Id for current response generated on server-side. it is useful to track any potential issues
        /// for this request.
        /// </summary>
        /// <returns>request Id generated on server-side</returns>
        public String GetRequestId()
        {
            String requestId = String.Empty;         
            _headers.TryGetValue(LogConsts.NAME_HEADER_REQUESTID, out requestId);
            return requestId;
        }

        /// <summary>
        /// Get all the http response headers
        /// </summary>
        /// <returns>Key-pair map for http headers</returns>
        public Dictionary<String, String> GetAllHeaders()
        {
            return new Dictionary<String, String>(_headers);
        }

        //internal helper function to consolidate logic to throw exception when parsing json string in http response.
        internal void ParseResponseBody(JObject jsonBody)
        {
            try
            {
                DeserializeFromJsonInternal(jsonBody);
            }
            catch (Exception ex)
            {
                throw new LogException("LOGBadResponse", "The response is not valid json string : " + jsonBody, ex, GetRequestId());
            }
        }
        internal void ParseResponseBody(JArray jsonBody)
        {
            try
            {
                DeserializeFromJsonInternal(jsonBody);
            }
            catch (Exception ex)
            {
                throw new LogException("LOGBadResponse", "The response is not valid json string : " + jsonBody, ex, GetRequestId());
            }
        }

        internal virtual void DeserializeFromJsonInternal(JObject json) { }
        internal virtual void DeserializeFromJsonInternal(JArray json) { }
    }
}
