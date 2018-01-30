using Aliyun.Api.LOG.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aliyun.Api.LOG.Response
{
    public class ListShardsResponse: LogResponse
    {
        private List<int> _shards;

        public ListShardsResponse(IDictionary<String, String> httpHeaders, JArray body)
            : base(httpHeaders)
        {
            ParseResponseBody(body);
        }

        public List<int> Shards
        {
            get { return _shards; }
        }
        internal override void DeserializeFromJsonInternal(JArray json) 
        {
            _shards = new List<int>();
            foreach(JObject obj in json.Children<JObject>())
            {
                _shards.Add(int.Parse(obj.GetValue("shardID").ToString()));
            }
        }
    }
}
