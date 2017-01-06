/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Diagnostics;
using Aliyun.Api.LOG.Common.Communication;

namespace Aliyun.Api.LOG.Common.Handlers
{
    /// <summary>
    /// Description of ResponseHandler.
    /// </summary>
    internal class ResponseHandler : IResponseHandler
    {
        public ResponseHandler()
        {
        }

        public virtual void Handle(ServiceResponse response)
        {
            Debug.Assert(response != null);
        }
    }
}
