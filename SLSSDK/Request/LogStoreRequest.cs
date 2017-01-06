using Aliyun.Api.LOG.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aliyun.Api.LOG.Request
{
    public abstract class LogStoreRequest : LogRequest
    {
        private String _logstore;

        /// <summary>
        /// The logstore name
        /// </summary>
        public String Logstore
        {
            get { return _logstore; }
            set { _logstore = value; }
        }
        public LogStoreRequest(String project, String logstore)
            : base(project)
        {
            Logstore = logstore;
        }
        abstract public void AddSpecParamsTo(IDictionary<String, String> dic);
        abstract public void AddSpecHeadersTo(IDictionary<String, String> dic);
    }
}
