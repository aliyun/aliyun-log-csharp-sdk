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
    /// The response of the ListLogStore API from sls server
    /// </summary>
    public class ListLogstoresResponse : LogResponse
    {
        private int _count;
        private List<String> _logstores;

        /// <summary>
        /// constructor with http header and body from response
        /// </summary>
        /// <param name="headers">http header from respsone</param>
        /// <param name="jsonBody">http body (in json) from response</param>
        public ListLogstoresResponse(IDictionary<String, String> headers, JObject jsonBody)
            :base(headers)
        {
            ParseResponseBody(jsonBody);
        }

        /// <summary>
        /// Count of the logstores
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// All of the logstores
        /// </summary>
        public List<String> Logstores
        {
            get { return _logstores; }
        }

        internal override void DeserializeFromJsonInternal(JObject json)
        {
            _count = (int)json[LogConsts.NAME_LISTLOGSTORE_TOTAL];
            _logstores = JsonConvert.DeserializeObject<List<string>>(json[LogConsts.NAME_LISTLOGSTORE_ITEM].ToString());
        }

    }
}
