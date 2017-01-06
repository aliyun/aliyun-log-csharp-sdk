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

namespace Aliyun.Api.LOG.Data
{
    /// <summary>
    /// This class presents one log content in logItem
    /// </summary>
    public class LogContent
    {
        private String _key = String.Empty;
        private String _value = String.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public LogContent()
        {

        }

        /// <summary>
        /// constructure with specified parameters
        /// </summary>
        /// <param name="key">log content's key </param>
        /// <param name="value">log content's value </param>
        public LogContent(String key, String value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// the logcontent's key
        /// </summary>
        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// the logcontent's value
        /// </summary>
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
