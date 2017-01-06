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
    /// The response of the GetLog API from sls server
    /// </summary>
    public class GetLogsResponse : LogResponse
    {
        private Int64 _count;
        private String _progress;
        private List<QueriedLog> _logs;

        /// <summary>
        /// constructor with http header and body from response
        /// </summary>
        /// <param name="headers">http header from response</param>
        /// <param name="jsonBody">http body (in json) from response</param>
        public GetLogsResponse(IDictionary<String, String> headers, JArray jsonBody)
            :base(headers)
        {
            String count;
            if(headers.TryGetValue(LogConsts.NAME_HEADER_X_LOG_COUNT, out count))
            {
                _count = Int64.Parse(count);
            }
            headers.TryGetValue(LogConsts.NAME_HEADER_X_LOG_PROGRESS, out _progress);
            ParseResponseBody(jsonBody);
        }

        /// <summary>
        /// The count of logs
        /// </summary>
        public Int64 Count
        {
            get { return _count; }
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
        /// List of logs
        /// </summary>
        public List<QueriedLog> Logs
        {
            get { return _logs; }
        }

        internal override void DeserializeFromJsonInternal(JArray json)
        {
            _logs = QueriedLog.DeserializeFromJson(json);
        }

        //used only in testing project
        internal String Print()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("{count:" + _count + "," + "progress:" + _progress + ",");
            if (_logs != null)
            {
                strBuilder.Append("{");
                foreach (QueriedLog log in _logs)
                    strBuilder.Append("[" + log.Print() + "]");
                strBuilder.Append("}");
            }
            strBuilder.Append("}");
            return strBuilder.ToString();
        }

    }
}
