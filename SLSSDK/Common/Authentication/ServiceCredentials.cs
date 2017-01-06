/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;

namespace Aliyun.Api.LOG.Common.Authentication
{
    /// <summary>
    /// Represents the credentials used to access Aliyun Open Services.
    /// </summary>
    internal class ServiceCredentials
    {
        /// <summary>
        /// Gets the access ID.
        /// </summary>
        public string AccessId { get; private set; }

        /// <summary>
        /// Gets the access key.
        /// </summary>
        public string AccessKey { get; private set; }

        /// <summary>
        /// Initialize an new instance of <see cref="ServiceCredentials"/>.
        /// </summary>
        /// <param name="accessId">The access ID.</param>
        /// <param name="accessKey">The access key.</param>
        public ServiceCredentials(string accessId, string accessKey)
        {
            if (string.IsNullOrEmpty(accessId))
                throw new ArgumentException(Aliyun.Api.LOG.Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");

            AccessId = accessId;
            AccessKey = accessKey;
        }
    }
}
