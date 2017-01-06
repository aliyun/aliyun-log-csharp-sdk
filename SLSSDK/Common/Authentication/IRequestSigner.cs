/*
 * Created by SharpDevelop.
 * User: xiaoming.yin
 * Date: 2012/5/30
 * Time: 14:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Aliyun.Api.LOG.Common.Communication;

namespace Aliyun.Api.LOG.Common.Authentication
{
    /// <summary>
    /// Description of IRequestSigner.
    /// </summary>
    internal interface IRequestSigner
    {
        /// <summary>
        /// Signs a request.
        /// </summary>
        /// <param name="request">The request to sign.</param>
        /// <param name="credentials">The credentials used to sign.</param>
        void Sign(ServiceRequest request, ServiceCredentials credentials);
    }
}
