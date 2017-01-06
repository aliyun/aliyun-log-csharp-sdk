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

namespace Aliyun.Api.LOG.Request
{
    /// <summary>
    /// The request used to send data to sls server
    /// Note: if source is not set explicitly, machine's local private ip is used
    /// </summary>
    public class PutLogsRequest : LogRequest
    {
        private String _logstore;
        private String _topic;
        private String _source;
        private List<LogItem> _logItems;

        /// <summary>
        /// default constructor.
        /// please set required fileds(project, logstore) initialized by this default constructor before  
        /// using it to send request. Otherwise, request will be failed with exception.
        /// </summary>
        public PutLogsRequest()
        {

        }

        /// <summary>
        /// constructor with all required fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        public PutLogsRequest(String project, String logstore)
            : base(project)
        {
            _logstore = logstore;
        }

        /// <summary>
        /// constructor with all possilbe fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        /// <param name="topic">log topic</param>
        /// <param name="source">log source</param>
        /// <param name="items">log data</param> 
        public PutLogsRequest(String project, String logstore, String topic, String source, List<LogItem> items)
            :base(project)
        {
            _logstore = logstore;
            _topic = topic;
            _source = source;
            _logItems = items;
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
        /// The log topic
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
        /// The log source
        /// </summary>
        public String Source
        {
            get { return _source; }
            set { _source = value; }
        }

        internal bool IsSetSource()
        {
            return _source != null;
        }

        /// <summary>
        /// List of logs
        /// </summary>
        public List<LogItem> LogItems
        {
            get { return _logItems; }
            set { _logItems = value; }
        }

        internal bool IsSetLogItems()
        {
            return _logItems != null;
        }
    }
}
