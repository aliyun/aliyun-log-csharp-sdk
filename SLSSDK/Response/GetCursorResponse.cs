using Aliyun.Api.LOG.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aliyun.Api.LOG.Response
{
    public class GetCursorResponse : LogResponse
    {
        private String _cursor;
        public String Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }
        public GetCursorResponse(IDictionary<String, String> headers, String cursor)
            : base(headers)
        {
            Cursor = cursor;
        }
    }
}
