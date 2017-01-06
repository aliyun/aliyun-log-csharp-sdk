/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aliyun.Api.LOG.Utilities;

namespace Aliyun.Api.LOG.Data
{
    /// <summary>
    /// QuriedLog used to present a log in query result. It contains log time, log source(ip/hostname,e.g),
    /// and multiple of key/value pairs to present the log content
    /// </summary>
    public class QueriedLog
    {
        private UInt32 _time;
        private String _source = String.Empty;
        private List<LogContent> _contents = new List<LogContent>();

        /// <summary>
        /// The log timestamp
        /// </summary>
        public UInt32 Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// The log source
        /// </summary>
        public String Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        /// List of key/value pair to present the log content
        /// </summary>
        public List<LogContent> Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public QueriedLog()
        {

        }

        internal static List<QueriedLog> DeserializeFromJson(JArray json)
        {
            List<QueriedLog> logs = new List<QueriedLog>();
            for (int i = 0; i < json.Count; ++i)
            {
                QueriedLog log = new QueriedLog();
                log._time = (UInt32)json[i][LogConsts.NAME_GETDATA_TIME];
                log._source = (String)json[i][LogConsts.NAME_GETDATA_SOURCE];
                log._contents = new List<LogContent>();
                foreach (var item in json[i].Children<JProperty>())
                {
                    if (item.Name.CompareTo(LogConsts.NAME_GETDATA_TIME) != 0 && item.Name.CompareTo(LogConsts.NAME_GETDATA_SOURCE) != 0)
                    {
                        log._contents.Add(new LogContent(item.Name, (String)json[i][item.Name]));
                    }
                }
                logs.Add(log);
            }
            return logs;
        }

        //used only in testing project
        internal String Print()
        {
            StringBuilder strBuilder = new StringBuilder();
            if (_contents != null)
            {
                for (int i = 0; i < _contents.Count; ++i)
                {
                    strBuilder.Append("(" + _contents[i].Key + "," + _contents[i].Value + ")");
                }
            }

            return strBuilder.ToString();
        }
    }
}
