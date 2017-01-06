using Aliyun.Api.LOG.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aliyun.Api.LOG.Request
{
    public class BatchGetLogsRequest: LogStoreRequest
    {
        private int _shard;
        private int _count;
        private String _cursor;
        public int Shard
        {
            get { return _shard; }
            set { _shard = value; }
        }
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public String Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }
        public BatchGetLogsRequest(String project, String logstore, int shard, String cursor, int count)
            : base(project, logstore)
        {
            Shard = shard;
            Cursor = cursor;
            Count = count;
        }
        override public void AddSpecParamsTo(IDictionary<String, String> dic)
        {
            dic.Add("type", "log");
            dic.Add("cursor", Cursor);
            dic.Add("count", _count.ToString());
        }
        override public void AddSpecHeadersTo(IDictionary<String, String> dic)
        {
            dic.Add(LogConsts.NAME_HEADER_ACCEPT_ENCODING, LogConsts.VALUE_HEADER_COMPRESSTYPE_LZ4);
            dic.Add(LogConsts.NAME_HEADER_ACCEPT, LogConsts.PBVALUE_HEADER_CONTENTTYPE);
        }
    }
}
