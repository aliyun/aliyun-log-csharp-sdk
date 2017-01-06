/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;
using Aliyun.Api.LOG.Utilities;

namespace Aliyun.Api.LOG.Utilities
{
    /// <summary>
    /// 表示访问阿里云服务的配置信息。
    /// </summary>
    internal class ClientConfiguration : ICloneable
    {
        private string _userAgent = LogConsts.CONST_USER_AGENT_PREFIX + typeof(ClientConfiguration).Assembly.GetName().Version.ToString();

        private int _connectionTimeout = LogConsts.DEFAULT_SLS_CONNECT_TIMEOUT;
        private int _maxErrorRetry = LogConsts.DEFAULT_SLS_RETRY_TIME;
        private int _readWrtTimeout = LogConsts.DEFAULT_SLS_READWRT_TIMEOUT;
        private int _retryInterval = LogConsts.DEFAULT_SLS_RETRY_INTERVALBASE;

        public int RetryIntervalBase
        {
            get { return _retryInterval; }
            set { _retryInterval = value; }
        }

        public int ReadWriteTimeout
        {
            get { return _readWrtTimeout; }
            set { _readWrtTimeout = value; }
        }

        /// <summary>
        /// 获取设置访问请求的User-Agent。
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        /// <summary>
        /// 获取或设置代理服务器的地址。
        /// </summary>
        public string ProxyHost { get; set; }

        /// <summary>
        /// 获取或设置代理服务器的端口。
        /// </summary>
        public int ProxyPort {get; set; }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// 获取或设置密码。
        /// </summary>
        public string ProxyPassword { get; set; }

        /// <summary>
        /// 获取或设置代理服务器授权用户所在的域。
        /// </summary>
        public string ProxyDomain { get; set; }

        /// <summary>
        /// 获取或设置连接的超时时间，单位为毫秒。
        /// </summary>
        public int ConnectionTimeout
        {
            get { return _connectionTimeout; }
            set { _connectionTimeout = value; }
        }

        /// <summary>
        /// 获取或设置请求发生错误时最大的重试次数。
        /// </summary>
        public int MaxErrorRetry
        {
            get { return _maxErrorRetry; }
            set { _maxErrorRetry = value; }
        }
        /// <summary>
        /// 初始化新的<see cref="ClientConfiguration"/>的实例。
        /// </summary>
        public ClientConfiguration()
        {
        }

        /// <summary>
        /// 获取该实例的拷贝。
        /// </summary>
        /// <returns>该实例的拷贝。</returns>
        public object Clone()
        {
            ClientConfiguration config = new ClientConfiguration();
            config.ConnectionTimeout = this.ConnectionTimeout;
            config.MaxErrorRetry = this.MaxErrorRetry;
            config.ProxyDomain = this.ProxyDomain;
            config.ProxyHost = this.ProxyHost;
            config.ProxyPassword = this.ProxyPassword;
            config.ProxyPort = this.ProxyPort;
            config.ProxyUserName = this.ProxyUserName;
            config.UserAgent = this.UserAgent;
            return config;
        }
    }
}
