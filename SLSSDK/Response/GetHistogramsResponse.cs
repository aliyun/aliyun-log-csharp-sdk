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
using Aliyun.Api.LOG.Data;
using Aliyun.Api.LOG.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aliyun.Api.LOG.Response
{
    /// <summary>
    /// The response of the GetHistogram API from sls server
    /// </summary>
    public class GetHistogramsResponse : LogResponse
    {
        private String _progress;
        private Int64 _count;
        private List<Histogram> _histograms;

        /// <summary>
        /// constructor with http header and body from response
        /// </summary>
        /// <param name="headers">http header from respsone</param>
        /// <param name="jsonBody">http body (in json) from response</param>
        public GetHistogramsResponse(IDictionary<String, String> headers, JArray jsonBody)
            :base(headers)
        {
            String count;
            if (headers.TryGetValue(LogConsts.NAME_HEADER_X_LOG_COUNT, out count))
            {
                _count = Int64.Parse(count);
            }
            headers.TryGetValue(LogConsts.NAME_HEADER_X_LOG_PROGRESS, out _progress);
            ParseResponseBody(jsonBody);
        }

        /// <summary>
        /// detect whether response are complete or not.
        /// </summary>
        /// <returns>true if response is complete. otherwise return false</returns>
        public bool IsCompleted()
        {
            return _progress == LogConsts.STATUS_COMPLETE;
        }

        /// <summary>
        /// The count of histograms
        /// </summary>
        public Int64 TotalCount
        {
            get { return _count; }
        }
        /// <summary>
        /// All of histograms
        /// </summary>
        public List<Histogram> Histograms
        {
            get { return  _histograms; }
        }

        internal override void DeserializeFromJsonInternal(JArray json)
        {
            _histograms = Histogram.DeserializeFromJson(json);
        }
    }
}
