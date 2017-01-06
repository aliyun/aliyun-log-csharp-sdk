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

namespace Aliyun.Api.LOG.Request
{
    /// <summary>
    /// The Request used to get data of a query from sls server
    /// </summary>
    public class GetLogsRequest : LogRequest
    {
        private String _logstore;
        private String _topic;
        private uint? _from;
        private uint? _to;
        private String _query;
        private int? _lines;
        private long? _offset;
        private bool? _reverse;

       /// <summary>
        /// default constructor.
        /// please set required fileds(project, logstore, from, to) initialized by this default constructor before  
        /// using it to send request. Otherwise, request will be failed with exception.
        /// </summary>
        public GetLogsRequest()
        {

        }

        /// <summary>
        /// constructor with all required fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        /// <param name="from">begin timestamp of time range to query</param>
        /// <param name="to">end timestamp of time range to query</param>
        public GetLogsRequest(String project, String logstore, uint from, uint to)
            :base(project)
        {
            _logstore = logstore;
            _from = from;
            _to = to;
        }

        /// <summary>
        /// constructor with all possible fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        /// <param name="from">begin timestamp of time range to query</param>
        /// <param name="to">end timestamp of time range to query</param>
        /// <param name="topic">log topic to query</param>
        /// <param name="query">query string to run</param>
        /// <param name="lines">count of logs to request</param>
        /// <param name="offset">offset of logs to request</param>
        /// <param name="reverse">flag indicates whether logs in response are in reversed order</param>
        public GetLogsRequest(String project, String logstore, uint from, uint to, String topic, String query, 
            int lines, int offset, bool reverse)
            :base(project)
        {
            _logstore = logstore;
            _from = from;
            _to = to;
            _topic = topic;
            _query = query;
            _lines = lines;
            _offset = offset;
            _reverse = reverse;
        }

        /// <summary>
        /// The logstore name
        /// </summary>
        public String Logstore
        {
            get { return _logstore; }
            set { _logstore = value; }
        }

        internal bool IsSetLogstore()
        {
            return _logstore != null;
        }

        /// <summary>
        /// The log topic to query
        /// </summary>
        public String Topic
        {
            get { return _topic; }
            set { _topic = value; }
        }

        internal bool IsSetTopic()
        {
            return _topic != null;
        }

        /// <summary>
        /// The begin timestamp of time range to query
        /// </summary>
        public uint From
        {
            get { return _from ?? default(uint); }
            set { _from = value; }
        }

        internal bool IsSetFrom()
        {
            return _from.HasValue;
        }

        /// <summary>
        /// The end timestamp of time range to query
        /// </summary>
        public uint To
        {
            get { return _to ?? default(uint); }
            set { _to = value; }
        }

        internal bool IsSetTo()
        {
            return _to.HasValue;
        }

        /// <summary>
        /// The query string to run
        /// </summary>
        public String Query
        {
            get { return _query; }
            set { _query = value; }
        }

        internal bool IsSetQuery()
        {
            return _query != null;
        }

        /// <summary>
        /// The count of logs to request
        /// </summary>
        public int Lines
        {
            get { return _lines ?? default(int); }
            set { _lines = value; }
        }

        internal bool IsSetLines()
        {
            return _lines.HasValue;
        }

        /// <summary>
        /// The offset of logs to request
        /// </summary>
        public long Offset
        {
            get { return _offset ?? default(long); }
            set { _offset = value; }
        }

        internal bool IsSetOffset()
        {
            return _offset.HasValue;
        }

        /// <summary>
        /// flag of logs' order int response.
        /// If reverse is true, the query will return the latest logs.
        /// </summary>
        public bool Reverse
        {
            get { return _reverse ?? false; }
            set { _reverse = value; }
        }

        internal bool IsSetReverse()
        {
            return _reverse.HasValue;
        }
    }
}
