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
using Aliyun.Api.LOG.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aliyun.Api.LOG.Response
{
    /// <summary>
    /// The response of the ListTopic API from sls server
    /// </summary>
    public class ListTopicsResponse : LogResponse
    {
        private Int64 _count;
        private String _nextToken;
        private List<String> _topics;

        /// <summary>
        /// constructor with http header and body from response
        /// </summary>
        /// <param name="headers">http header from respsone</param>
        /// <param name="jsonBody">http body (in json) from response</param>
        public ListTopicsResponse(IDictionary<String, String> headers, JArray jsonBody)
            :base(headers)
        {
            headers.TryGetValue(LogConsts.NAME_HEADER_X_LOG_NEXT_TOKEN, out _nextToken);
            String tmpCount;
            if (headers.TryGetValue(LogConsts.NAME_HEADER_X_LOG_COUNT, out tmpCount))
            {
                _count = int.Parse(tmpCount);
            }
            ParseResponseBody(jsonBody);
        }

        /// <summary>
        /// The count of log topics in the response
        /// </summary>
        public Int64 Count
        {
            get { return _count; }
        }

        /// <summary>
        /// The next token property in the response. It is used to list more topics in next ListTopics request. 
        /// If there is no more topics to list, it will return an empty string.
        /// </summary>
        public String NextToken
        {
            get { return _nextToken; }
        }

        /// <summary>
        /// All log topics in the response
        /// </summary>
        public List<String> Topics
        {
            get { return _topics; }
        }
        internal override void DeserializeFromJsonInternal(JArray json) 
        {
            _topics = JsonConvert.DeserializeObject<List<string>>(json.ToString());
        }
    }
}
