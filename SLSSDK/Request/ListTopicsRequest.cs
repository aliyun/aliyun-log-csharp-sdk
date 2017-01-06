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
    /// The request used to list topic from sls server
    /// </summary>
    public class ListTopicsRequest : LogRequest
    {
        private String _logstore;
        private String _token;
        private int? _lines;

        /// <summary>
        /// default constructor.
        /// please set required fileds(project, logstore) initialized by this default constructor before using it to 
        /// send request. Otherwise, request will be failed with exception.
        /// </summary>
        public ListTopicsRequest()
        {

        }

        /// <summary>
        /// constructor with all required fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        public ListTopicsRequest(String project, String logstore)
            :base(project)
        {
            _logstore = logstore;
        }

        /// <summary>
        /// constructor with all possible fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        /// <param name="token">token to list more topics</param>
        /// <param name="lines">count of topics to request</param>
        public ListTopicsRequest(String project, String logstore, String token, Int32 lines)
            :base(project)
        {
            _logstore = logstore;
            _token = token;
            _lines = lines;
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
        /// The token to list more topics
        /// </summary>
        public String Token
        {
            get { return _token; }
            set { _token = value; }
        }

        internal bool IsSetToken()
        {
            return _token != null;
        }

        /// <summary>
        /// The count of topics to request
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
    }
}
