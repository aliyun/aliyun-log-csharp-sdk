/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
using Aliyun.Api.LOG.Common.Authentication;
using Aliyun.Api.LOG.Common.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Aliyun.Api.LOG.Utilities
{
    internal class LogRequestSigner : IRequestSigner
    {
        private readonly String httpResource;
        private readonly HttpMethod httpMethod;
        
        public LogRequestSigner(String httpResource, HttpMethod httpMethod)
        {
            this.httpResource = httpResource;
            this.httpMethod = httpMethod;
        }
        
        public void Sign(ServiceRequest request, ServiceCredentials credentials)
        {
            request.Headers.Add(LogConsts.NAME_HEADER_AUTH, LogClientTools.Signature(request.Headers, request.Parameters, this.httpMethod, this.httpResource, credentials.AccessId, credentials.AccessKey));
        }
    }
}
