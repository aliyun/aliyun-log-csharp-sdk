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
    /// Super class of all request
    /// </summary>
    public class LogRequest
    {
        private String _project;

        /// <summary>
        /// default constructor of SLS Request.
        /// </summary>
        public LogRequest()
        {

        }

        /// <summary>
        /// LogRequest constructor with project name.
        /// </summary>
        /// <param name="project">project name to do SLS Request</param>
        public LogRequest(String project)
        {
            _project = project;
        }

        /// <summary>
        /// project name of the request
        /// </summary>
        public String Project 
        {
            get { return _project; }
            set { _project = value; }
        }

        internal bool IsSetProject()
        {
            return _project != null;
        }
    }
}
