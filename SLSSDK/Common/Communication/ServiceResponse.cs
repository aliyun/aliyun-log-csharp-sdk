/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
//#define __UT_TEST_0EC173788C65DD08DA60575219707632__
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Aliyun.Api.LOG.Common.Communication
{
    /// <summary>
    /// Represents the data of the responses of requests.
    /// </summary>
    internal abstract class ServiceResponse : ServiceMessage, IDisposable
    {
        public abstract HttpStatusCode StatusCode { 
            get; 
#if(__UT_TEST_0EC173788C65DD08DA60575219707632__) 
            set;
#endif
        }
        
        public abstract Exception Failure { get; }
        
        public virtual bool IsSuccessful()
        {
            return (int)this.StatusCode / 100 == (int)HttpStatusCode.OK / 100;
        }
        
        /// <summary>
        /// Throws the exception from communication if the status code is not 2xx.
        /// </summary>
        public virtual void EnsureSuccessful()
        {
            if (!IsSuccessful())
            {
                // Disposing the content should help users: If users call EnsureSuccessStatusCode(), an exception is
                // thrown if the response status code is != 2xx. I.e. the behavior is similar to a failed request (e.g.
                // connection failure). Users don't expect to dispose the content in this case: If an exception is
                // thrown, the object is responsible fore cleaning up its state.
                if (Content != null)
                {
                    Content.Dispose();
                }

                Debug.Assert(this.Failure != null);
                throw this.Failure;
            }
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceResponse()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
