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

namespace Aliyun.Api.LOG.Response
{
    /// <summary>
    /// The response of the PutLogs API from sls server
    /// </summary>
    public class PutLogsResponse : LogResponse
    {
        /// <summary>
        /// default constructor for PutLogsResponse
        /// </summary>
        /// <param name="header">header information in http response</param>
        public PutLogsResponse(IDictionary<String, String>header)
            :base(header)
        {

        }
    }
}
