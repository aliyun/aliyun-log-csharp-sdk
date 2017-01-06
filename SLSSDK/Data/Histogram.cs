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
    /// The log status(histogram info)
    /// </summary>
    public class Histogram
    {
        private UInt32 _from;
        private UInt32 _to;
        private Int64 _count;
        private String _progress;

        /// <summary>
        /// The begin timestamp of time range
        /// </summary>
        public UInt32 From
        {
            get { return _from; }
            set { _from = value; }
        }

        /// <summary>
        /// The end timestamp of time range
        /// </summary>
        public UInt32 To
        {
            get { return _to; }
            set { _to = value; }
        }

        /// <summary>
        /// The log count
        /// </summary>
        public Int64 Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// detect whether histogram is complete or not.
        /// </summary>
        /// <returns>true if return histogram is complete. otherwise return false</returns>
        public bool IsCompleted()
        {
            return _progress == LogConsts.STATUS_COMPLETE;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public Histogram()
        {

        }

        internal static List<Histogram> DeserializeFromJson(JArray json)
        {
            List<Histogram> hlst = new List<Histogram>();
            if (json != null)
            {
                for (int i = 0; i < json.Count; ++i)
                {
                    Histogram htg = new Histogram();
                    htg._from = (UInt32)json[i][LogConsts.NAME_GETSTATUS_FROM];
                    htg._to = (UInt32)json[i][LogConsts.NAME_GETSTATUS_TO];
                    htg._count = (UInt32)json[i][LogConsts.NAME_GETSTATUS_COUNT];
                    htg._progress = (String)json[i][LogConsts.NAME_GETSTATUS_PROGRESS];
                    hlst.Add(htg);
                }
            }
            return hlst;
        }
    }
}
