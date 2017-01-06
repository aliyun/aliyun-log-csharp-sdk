using Aliyun.Api.LOG.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aliyun.Api.LOG.Request
{
    public enum ShardCursorMode
    {
        BEGIN, END
    };
    public class GetCursorRequest : LogStoreRequest
    {
        private int _shard;
        private ShardCursorMode? _cursorMode;
        private uint? _cursorTime;
        public int Shard
        {
            get { return _shard; }
            set { _shard = value; }
        }
        public ShardCursorMode CursorMode
        {
            get { return _cursorMode ?? default(ShardCursorMode); }
            set { _cursorMode = value; }
        }
        public bool IsSetCursorMode()
        {
            return _cursorMode.HasValue;
        }
        public uint CursorTime
        {
            get { return _cursorTime ?? default(uint); }
            set { _cursorTime = value; }
        }
        public GetCursorRequest(String project, String logstore, int shard, ShardCursorMode mode)
            : base(project, logstore)
        {
            Shard = shard;
            CursorMode = mode;
        }
        public GetCursorRequest(String project, String logstore, int shard, uint time)
            : base(project, logstore)
        {
            Shard = shard;
            CursorTime = time;
        }
        override public void AddSpecParamsTo(IDictionary<String, String> dic)
        {
            dic.Add("type", "cursor");
            if (IsSetCursorMode())
            {
                dic.Add("from", CursorMode == ShardCursorMode.BEGIN ? "begin" : "end");
            }
            else
            {
                dic.Add("from", _cursorTime.ToString());
            }
        }
        override public void AddSpecHeadersTo(IDictionary<String, String> dic)
        {
        }
    }
}
