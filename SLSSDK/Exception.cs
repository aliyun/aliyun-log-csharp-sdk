/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;


namespace Aliyun.Api.LOG
{
    /// <summary>
    /// The Exception of the sls request and response.
    /// </summary>
    public class LogException : ApplicationException
    {
        private String _errorCode;
        private String _requestId;
        /// <summary>
        /// Get Sls sever requestid.
        /// </summary>
        public String RequestId {
            get {
                return _requestId;
            }
        }
        /// <summary>
        /// Get LogException error code.
        /// </summary>
        public String ErrorCode {
            get {
                return _errorCode;
            }
        }
        /// <summary>
        /// LogException constructor
        /// </summary>
        /// <param name="code">error code</param>
        /// <param name="message">error message</param>
        /// <param name="requestId">request identifier</param>
        public LogException(String code, String message,String requestId = "")
            : base(message) {
            _errorCode = code;
            _requestId = requestId;
        }
        /// <summary>
        /// LogException constructor
        /// </summary>
        /// <param name="code">error code</param>
        /// <param name="message">error message</param>
        /// <param name="innerException">the inner exception wrapped by LogException</param>
        /// <param name="requestId"></param>
        public LogException(String code, String message, Exception innerException, String requestId = "")
            : base(message, innerException)
        {
            _errorCode = code;
            _requestId = requestId;
        }

        /// <summary>
        /// get string presentation of LogException
        /// </summary>
        /// <returns>object in string</returns>
        public override String ToString()
        {
            String msgFormat = @"ErrorCode : {0}, Message: {1}, RequestId: {2}";
            return String.Format(msgFormat, _errorCode, Message, _requestId);
        }
    }
}
