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
    /// The request used to list log store from sls server
    /// </summary>
    public class ListLogstoresRequest : LogRequest
    {
     
        /// <summary>
        /// default constructor
        /// </summary>
        public ListLogstoresRequest()
        {

        }
        /// <summary>
        /// constructor with project name
        /// </summary>
        /// <param name="project">project name</param>
        public ListLogstoresRequest(String project): base(project){
        }
    }
}
