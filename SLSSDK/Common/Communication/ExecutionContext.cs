/*
 * Created by SharpDevelop.
 * User: xiaoming.yin
 * Date: 2012/5/30
 * Time: 14:21
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Aliyun.Api.LOG.Common.Authentication;
using Aliyun.Api.LOG.Common.Handlers;

namespace Aliyun.Api.LOG.Common.Communication
{
    /// <summary>
    /// Description of ExecutionContext.
    /// </summary>
    internal class ExecutionContext
    {
        /// <summary>
        /// The default encoding (charset name).
        /// </summary>
        private const string DefaultEncoding = "utf-8";
        
        private IList<IResponseHandler> _responseHandlers = new List<IResponseHandler>();

        /// <summary>
        /// Gets or sets the charset.
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// Gets or sets the request signer.
        /// </summary>
        public IRequestSigner Signer { get; set; }
        
        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public ServiceCredentials Credentials { get; set ;}
        
        /// <summary>
        /// Gets the list of <see cref="IResponseHandler" />.
        /// </summary>
        public IList<IResponseHandler> ResponseHandlers
        {
            get { return _responseHandlers; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExecutionContext()
        {
            this.Charset = DefaultEncoding;
        }

    }
}
