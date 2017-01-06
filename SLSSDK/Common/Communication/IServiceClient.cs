/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;

namespace Aliyun.Api.LOG.Common.Communication
{
    /// <summary>
    /// Represent the channel that communicates with an Aliyun Open Service.
    /// </summary>
    internal interface IServiceClient
    {
        /// <summary>
        /// Sends a request to the service.
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <param name="context">The execution context.</param>
        /// <returns>The response data.</returns>
        ServiceResponse Send(ServiceRequest request, ExecutionContext context);
    }
}
