using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aliyun.Api.LOG.Request
{
    public class ListShardsRequest: LogStoreRequest
    {
        public ListShardsRequest(String projrct, String logstore)
            : base(projrct, logstore)
        { 
        }
        override public void AddSpecParamsTo(IDictionary<String, String> dic)
        {
 
        }
        override public void AddSpecHeadersTo(IDictionary<String, String> dic)
        { 

        }
    }
}
