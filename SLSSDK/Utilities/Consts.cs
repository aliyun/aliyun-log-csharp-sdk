/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
using Aliyun.Api.LOG.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aliyun.Api.LOG.Utilities
{
    internal class LogConsts
    {
        public const string CONST_USER_AGENT_PREFIX = "log-dotnet-sdk-v-";

        public const int LIMIT_LOG_SIZE = 3 * 1024 * 1024;
        public const int LIMIT_LOG_COUNT = 4096;
        public const int DEFAULT_SLS_RETRY_TIME = 3;
        public const int DEFAULT_SLS_CONNECT_TIMEOUT = 5 * 1000;
        public const int DEFAULT_SLS_READWRT_TIMEOUT = 20 * 1000;
        public const int DEFAULT_SLS_RETRY_INTERVALBASE = 100;

        public const String NAME_ERROR_CODE = "errorCode";
        public const String NAME_ERROR_MESSAGE = "errorMessage";
        public const String NAME_LISTLOGSTORE_ITEM = "logstores";
        public const String NAME_LISTLOGSTORE_TOTAL = "count";
        public const String NAME_LISTTOPIC_COUNT = "count";
        public const String NAME_LISTTOPIC_TOPICS = "topics";
        public const String NAME_LISTTOPIC_NEXTTOKEN = "next_token";
        public const String NAME_GETSTATUS_PROGRESS = "progress";
        public const String NAME_GETSTATUS_COUNT = "count";
        public const String NAME_GETSTATUS_FROM = "from";
        public const String NAME_GETSTATUS_TO = "to";
        public const String NAME_GETSTATUS_HISTOGRAM = "histograms";
        public const String NAME_GETDATA_COUNT = "count";
        public const String NAME_GETDATA_PROGRESS = "progress";
        public const String NAME_GETDATA_LOGS = "logs";
        public const String NAME_GETDATA_TIME = "__time__";
        public const String NAME_GETDATA_SOURCE = "__source__";

        public const string NAME_MD5 = "MD5";

        public const String NAME_HTTP_GET = "GET";
        public const String NAME_HTTP_POST = "POST";
        public const String NAME_HTTP_PUT = "PUT";
        public const String NAME_HTTP_DELETE = "DELETE";
        public const String NAME_HTTP_PATCH = "PATCH";
        public const String NAME_HTTP_HEAD = "HEAD";
        public const String NAME_HTTP_OPTIONS = "OPTIONS";

        public const String NAME_HEADER_AUTH = HttpHeaders.Authorization;
        public const String PREFIX_VALUE_HEADER_AUTH = "LOG" + " ";
        public const String NAME_HEADER_CONTENTTYPE = HttpHeaders.ContentType;
        public const String JSONVALUE_HEADER_CONTENTTYPE = "application/json";
        public const String PBVALUE_HEADER_CONTENTTYPE = "application/x-protobuf";
        public const String NAME_HEADER_MD5 = HttpHeaders.ContentMd5;
        public const String NAME_HEADER_HOST = "Host";
        public const String NAME_HEADER_APIVERSION = "x-log-apiversion";
        public const String VALUE_HEADER_APIVERSION = "0.6.0";
        public const String NAME_HEADER_ACCESSKEYID = "x-log-accesskeyid";
        public const String NAME_HEADER_COMPRESSTYPE = "x-log-compresstype";
        public const String NAME_HEADER_REQUESTID = "x-log-requestid";
        public const String NAME_HEADER_DATE = "x-log-date";
        public const String NAME_HEADER_X_LOG_COUNT = "x-log-count";
        public const String NAME_HEADER_X_LOG_NEXT_TOKEN = "x-log-nexttoken";
	    public const String NAME_HEADER_X_LOG_PROGRESS = "x-log-progress";
        public const String NAME_HEADER_ACCEPT_ENCODING = "Accept-Encoding";
        public const String NAME_HEADER_ACCEPT = "Accept";
        public const String VALUE_HEADER_COMPRESSTYPE_DEFLATE = "deflate";
        public const String VALUE_HEADER_COMPRESSTYPE_LZ4 = "lz4";
        public const String NAME_HEADER_BODYRAWSIZE = "x-log-bodyrawsize";
        public const String NAME_HEADER_NEXT_CURSOR = "x-log-cursor";
        public const String NAME_HEADER_LOG_COUNT = "x-log-count";
        public const String NAME_HEADER_LOG_BODY_RAW_SIZE = "x-log-bodyrawsize";
        public const String NAME_HEADER_SIGMETHOD = "x-log-signaturemethod";
        public const String VALUE_HEADER_SIGMETHOD = "hmac-sha1";
        public const String NAME_HEADER_ACS_SECURITY_TOKEN = "x-acs-security-token";
        public const String RESOURCE_SEPARATOR = "/";
        public const String RESOURCE_LOGSTORES = RESOURCE_SEPARATOR + "logstores";
        public const String RESOURCE_SHARDS = RESOURCE_SEPARATOR + "shards";
        public const String PARAMETER_OFFSET = "offset";
        public const String PARAMETER_LINES = "line";
        public const String RESOURCE_TOPIC = "topic";
        public const String PARAMETER_TOKEN = "token";
        public const String PARAMETER_TYPE = "type";
        public const String VALUE_TYPE_CONTENT = "log";
        public const String VALUE_TYPE_STATUS = "histogram";
        public const String PARAMETER_TOPIC = "topic";
        public const String PARAMETER_FROM = "from";
        public const String PARAMETER_TO = "to";
        public const String PARAMETER_QUERY = "query";
        public const String PARAMETER_REVERSE = "reverse";

        public const String STATUS_COMPLETE = "Complete";
        public const String STATUS_INCOMPLETE = "InComplete";
    }
}
