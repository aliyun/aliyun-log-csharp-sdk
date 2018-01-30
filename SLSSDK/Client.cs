/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
using Aliyun.Api.LOG.Request;
using Aliyun.Api.LOG.Response;
using Aliyun.Api.LOG.Common.Authentication;
using Aliyun.Api.LOG.Common.Communication;
using Aliyun.Api.LOG.Common.Utilities;
using Aliyun.Api.LOG.Data;
using Aliyun.Api.LOG.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Aliyun.Api.LOG
{
    /// <summary>
    /// This is the main class in the sdk. It can be used to communicate with sls server to put/get/query data.
    /// </summary>
    public class LogClient
    {
        private ServiceClient serviceClient;
        private Object _slsClientLockObj = new Object();

        private String _accessKeyId;
        private String _accessKey;
        
        private String _endpoint;
        private String _hostName;
        private String _uriScheme;
        private int _port;

        private String _localMachinePrivateIp;
        private String _securityToken;
        /// <summary>
        /// readonly property, AccessKeyId of LogClient
        /// </summary>
        public String AccessKeyId { get { return _accessKeyId; } }

        /// <summary>
        /// readonly property, AccessKey of LogClient
        /// </summary>
        public String AccessKey { get { return _accessKey; } }


        /// <summary>
        /// readonly property, Endpoint of LogClient
        /// </summary>
        public String Endpoint { get { return _endpoint; } }

        /// <summary>
        /// Read/Write Timeouf for underlying HTTPWebRequest.ReadWriteTimeout
        /// </summary>
        public int ReadWriteTimeout
        {
            get 
            { 
                return serviceClient.Configuration.ReadWriteTimeout; 
            }
            set
            {
                if (value > 0)
                {
                    lock (_slsClientLockObj)
                        serviceClient.Configuration.ReadWriteTimeout = value;
                }
            }
        }

        /// <summary>
        /// Connection Timeout for underlying HttpWebRequest.Timeout
        /// </summary>
        public int ConnectionTimeout
        {
            get 
            { 
                return serviceClient.Configuration.ConnectionTimeout; 
            }
            set
            {
                if (value > 0) 
                {
                    lock (_slsClientLockObj)
                        serviceClient.Configuration.ConnectionTimeout = value;
                }
            }
        }

        /// <summary>
        /// Construct the sls client with accessId, accessKey and server address, 
        /// all other parameters will be set to default value
        /// </summary>
        /// <param name="endpoint">the sls server address(e.g.,http://cn-hangzhou.sls.aliyuncs.com)</param>
        /// <param name="accessKeyId">aliyun accessId</param>
        /// <param name="accessKey">aliyun accessKey</param>
        public LogClient(String endpoint, String accessKeyId, String accessKey)
        {
            if (!endpoint.StartsWith("http://") && !endpoint.StartsWith("https://"))
                endpoint = "http://" + endpoint;
            setEndpoint(endpoint);
            if (IpUtils.IsIpAddress(this._hostName))
                throw new LogException("LogClientError", "client error happens");

            _localMachinePrivateIp = IpUtils.GetLocalMachinePrivateIp();
            _accessKeyId = accessKeyId;
            _accessKey = accessKey;

            serviceClient = ServiceClient.Create(new ClientConfiguration());
            serviceClient.Configuration.ConnectionTimeout = LogConsts.DEFAULT_SLS_CONNECT_TIMEOUT;
            serviceClient.Configuration.ReadWriteTimeout = LogConsts.DEFAULT_SLS_READWRT_TIMEOUT;
        }

        /// <summary>
        /// Construct the sls client with accessId, accessKey and server address, 
        /// all other parameters will be set to default value
        /// </summary>
        /// <param name="endpoint">the sls server address(e.g.,http://cn-hangzhou.sls.aliyuncs.com)</param>
        /// <param name="accessKeyId">aliyun accessId</param>
        /// <param name="accessKey">aliyun accessKey</param>
        public LogClient(String endpoint, String accessKeyId, String accessKey, String securityToken)
        {
            if (!endpoint.StartsWith("http://") && !endpoint.StartsWith("https://"))
                endpoint = "http://" + endpoint;
            setEndpoint(endpoint);
            if (IpUtils.IsIpAddress(this._hostName))
                throw new LogException("LogClientError", "client error happens");

            _localMachinePrivateIp = IpUtils.GetLocalMachinePrivateIp();
            _accessKeyId = accessKeyId;
            _accessKey = accessKey;
            _securityToken = securityToken;
            serviceClient = ServiceClient.Create(new ClientConfiguration());
            serviceClient.Configuration.ConnectionTimeout = LogConsts.DEFAULT_SLS_CONNECT_TIMEOUT;
            serviceClient.Configuration.ReadWriteTimeout = LogConsts.DEFAULT_SLS_READWRT_TIMEOUT;
        }
        public GetCursorResponse GetCursor(GetCursorRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                sReq.ResourcePath = LogConsts.RESOURCE_LOGSTORES
                    + LogConsts.RESOURCE_SEPARATOR
                    + request.Logstore
                    + LogConsts.RESOURCE_SHARDS
                    + LogConsts.RESOURCE_SEPARATOR
                    + request.Shard;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                request.AddSpecHeadersTo(sReq.Headers);
                request.AddSpecParamsTo(sReq.Parameters);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JObject body = LogClientTools.ParserResponseToJObject(response.Content);
                    GetCursorResponse getCursorResp = new GetCursorResponse(response.Headers, body.GetValue("cursor").ToString());
                    return getCursorResp;
                }
            }
        }
        public ListShardsResponse ListShards(ListShardsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                sReq.ResourcePath = LogConsts.RESOURCE_LOGSTORES
                    + LogConsts.RESOURCE_SEPARATOR
                    + request.Logstore
                    + LogConsts.RESOURCE_SHARDS;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                request.AddSpecHeadersTo(sReq.Headers);
                request.AddSpecParamsTo(sReq.Parameters);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JArray body = LogClientTools.ParserResponseToJArray(response.Content);
                    ListShardsResponse listShardsResp = new ListShardsResponse(response.Headers, body);
                    return listShardsResp;
                }
            }
        }
        public BatchGetLogsResponse BatchGetLogs(BatchGetLogsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                sReq.ResourcePath = LogConsts.RESOURCE_LOGSTORES
                    + LogConsts.RESOURCE_SEPARATOR
                    + request.Logstore
                    + LogConsts.RESOURCE_SHARDS
                    + LogConsts.RESOURCE_SEPARATOR
                    + request.Shard;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                request.AddSpecHeadersTo(sReq.Headers);
                request.AddSpecParamsTo(sReq.Parameters);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    BatchGetLogsResponse batchGetLogsResp = new BatchGetLogsResponse(response.Headers, response.Content);
                    return batchGetLogsResp;
                }
            }
        }
        /// <summary>
        /// List all of the logstores under specified project
        /// </summary>
        /// <param name="request">The request to list logstores</param>
        /// <exception>LogException</exception>
        /// <returns>The response of list log logstores</returns>
        public ListLogstoresResponse ListLogstores(ListLogstoresRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);
                sReq.ResourcePath = LogConsts.RESOURCE_LOGSTORES;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JObject body = LogClientTools.ParserResponseToJObject(response.Content);
                    ListLogstoresResponse res = new ListLogstoresResponse(response.Headers, body);
                    return res;
                }
            }
        }
     
        /// <summary>
        /// put logs into sls server
        /// </summary>
        /// <param name="request">The request to put logs </param>
        /// <exception>LogException</exception>
        /// <returns>The response to put logs</returns>
        public PutLogsResponse PutLogs(PutLogsRequest request) 
        {
            LogGroup.Builder lgBuilder = LogGroup.CreateBuilder();
            
            if (request.IsSetTopic())
                lgBuilder.Topic = request.Topic;

            if (request.IsSetSource())
                lgBuilder.Source = request.Source;
            else
                lgBuilder.Source = _localMachinePrivateIp;  //use default machine private ip as source (should we

            if (request.IsSetLogItems())
            {
                foreach (var item in request.LogItems)
                {
                    Log.Builder logBuilder = Log.CreateBuilder();
                    logBuilder.Time = item.Time;
                    foreach (var kv in item.Contents)
                    {
                        Log.Types.Content.Builder contentBuilder = Log.Types.Content.CreateBuilder();
                        contentBuilder.Key = kv.Key;
                        contentBuilder.Value = kv.Value;
                        logBuilder.AddContents(contentBuilder);
                    }
                    lgBuilder.AddLogs(logBuilder);
                }
            }

            return PutLogs(request, lgBuilder.Build());
        }

        internal PutLogsResponse PutLogs(PutLogsRequest request, LogGroup logGroup)
        {
            if (logGroup.LogsCount > LogConsts.LIMIT_LOG_COUNT)
                throw new LogException("InvalidLogSize", "logItems' length exceeds maximum limitation： " + LogConsts.LIMIT_LOG_COUNT + " lines.");
            else if(logGroup.SerializedSize > LogConsts.LIMIT_LOG_SIZE)
                throw new LogException("InvalidLogSize", "logItems' size exceeds maximum limitation: " + LogConsts.LIMIT_LOG_SIZE + " byte.");
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Post;
                sReq.Endpoint = BuildReqEndpoint(request);

                //use empty string to replace Logstore if not set by user explicitly
                string logstore = request.IsSetLogstore() ? request.Logstore : String.Empty;
                sReq.ResourcePath = LogConsts.RESOURCE_LOGSTORES + LogConsts.RESOURCE_SEPARATOR + logstore;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);
                sReq.Headers.Add(LogConsts.NAME_HEADER_CONTENTTYPE, LogConsts.PBVALUE_HEADER_CONTENTTYPE);
                byte[] logBytes = logGroup.ToByteArray();
                sReq.Headers[LogConsts.NAME_HEADER_BODYRAWSIZE] = logBytes.Length.ToString();
                sReq.Headers.Add(LogConsts.NAME_HEADER_COMPRESSTYPE, LogConsts.VALUE_HEADER_COMPRESSTYPE_LZ4);
                logBytes = LogClientTools.CompressToLz4(logBytes);
                sReq.Headers.Add(LogConsts.NAME_HEADER_MD5, LogClientTools.GetMd5Value(logBytes));
                sReq.Content = new MemoryStream(logBytes);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Post);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    PutLogsResponse putLogResp = new PutLogsResponse(response.Headers);
                    return putLogResp;
                }
            }
           
        }

        /// <summary>
        /// Get the topics in the logtstore
        /// </summary>
        /// <param name="request">The list topics request</param>
        /// <exception>LogException</exception>
        /// <returns>The List topics response</returns>
        public ListTopicsResponse ListTopics(ListTopicsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                //use empty string to replace Logstore if not set by user explicitly
                string logstore = request.IsSetLogstore() ? request.Logstore : String.Empty;
                sReq.ResourcePath = LogConsts.RESOURCE_LOGSTORES + LogConsts.RESOURCE_SEPARATOR + logstore;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                sReq.Parameters.Add(LogConsts.PARAMETER_TYPE, LogConsts.RESOURCE_TOPIC);

                if (request.IsSetToken())
                    sReq.Parameters.Add(LogConsts.PARAMETER_TOKEN, request.Token);

                if (request.IsSetLines())
                    sReq.Parameters.Add(LogConsts.PARAMETER_LINES, request.Lines.ToString());

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JArray body = LogClientTools.ParserResponseToJArray(response.Content);
                    ListTopicsResponse res = new ListTopicsResponse(response.Headers, body);
                    return res;
                }
            }
        }

        /// <summary>
        ///  Get The sub set of logs data from sls server which match input
        ///  parameters. 
        /// </summary>
        /// <param name="request">The get logs request</param>
        /// <exception>LogException</exception>
        /// <returns>The get Logs response</returns>
        public GetLogsResponse GetLogs(GetLogsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                //use empty string to replace Logstore if not set by user explicitly
                string logstore = request.IsSetLogstore() ? request.Logstore : String.Empty;
                sReq.ResourcePath = LogConsts.RESOURCE_LOGSTORES + LogConsts.RESOURCE_SEPARATOR + logstore;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                sReq.Parameters.Add(LogConsts.PARAMETER_TYPE, LogConsts.VALUE_TYPE_CONTENT);

                if (request.IsSetTopic())
                    sReq.Parameters.Add(LogConsts.PARAMETER_TOPIC, request.Topic);

                if (request.IsSetFrom())
                    sReq.Parameters.Add(LogConsts.PARAMETER_FROM, request.From.ToString());

                if (request.IsSetTo())
                    sReq.Parameters.Add(LogConsts.PARAMETER_TO, request.To.ToString());

                if (request.IsSetQuery())
                    sReq.Parameters.Add(LogConsts.PARAMETER_QUERY, request.Query);

                if (request.IsSetLines())
                    sReq.Parameters.Add(LogConsts.PARAMETER_LINES, request.Lines.ToString());

                if (request.IsSetOffset())
                    sReq.Parameters.Add(LogConsts.PARAMETER_OFFSET, request.Offset.ToString());

                if (request.IsSetReverse())
                    sReq.Parameters.Add(LogConsts.PARAMETER_REVERSE, request.Reverse.ToString());

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JArray body = LogClientTools.ParserResponseToJArray(response.Content);
                    GetLogsResponse res = new GetLogsResponse(response.Headers, body);
                    return res;
                }
            }
        }

        /// <summary>
        /// Get The log status(histogram info) from sls server which match input
        /// parameters. All the logs with logstore and topic in [from, to) which
        /// contain the keys in query are the matched data.
        /// </summary>
        /// <param name="request">The get histograms request</param>
        /// <exception>LogException</exception>
        /// <returns>The get histograms response</returns>
        public GetHistogramsResponse GetHistograms(GetHistogramsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                //use empty string to replace Logstore if not set by user explicitly
                string logstore = request.IsSetLogstore() ? request.Logstore : String.Empty;
                sReq.ResourcePath = LogConsts.RESOURCE_LOGSTORES + LogConsts.RESOURCE_SEPARATOR + logstore;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                sReq.Parameters.Add(LogConsts.PARAMETER_TYPE, LogConsts.VALUE_TYPE_STATUS);

                if (request.IsSetTopic())
                    sReq.Parameters.Add(LogConsts.PARAMETER_TOPIC, request.Topic);

                if (request.IsSetFrom())
                    sReq.Parameters.Add(LogConsts.PARAMETER_FROM, request.From.ToString());

                if (request.IsSetTo())
                    sReq.Parameters.Add(LogConsts.PARAMETER_TO, request.To.ToString());

                if (request.IsSetQuery())
                    sReq.Parameters.Add(LogConsts.PARAMETER_QUERY, request.Query);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JArray body = LogClientTools.ParserResponseToJArray(response.Content);
                    GetHistogramsResponse res = new GetHistogramsResponse(response.Headers, body);
                    return res;
                }
            }
        }

        //used for unit testing
        internal void SetWebSend(ServiceClientImpl.WebSend send)
        {
            ((ServiceClientImpl)serviceClient).SendMethod = send;
        }

        //set endpoint of service. It may throw exceptions if endpoint is not in valid format.
        private void setEndpoint(String endpoint)
        {
            try
            {
                Uri endpointUri = new Uri(endpoint);
                _endpoint = endpointUri.ToString();
                _hostName = endpointUri.Host;
                _uriScheme = endpointUri.Scheme;
                _port = endpointUri.Port;
            }
            catch (Exception) {
                throw new LogException("LogClientError", "client error happens");
            }
        }

        private void FillCommonHeaders(ServiceRequest sReq)
        {
            sReq.Headers.Add(LogConsts.NAME_HEADER_DATE, DateUtils.FormatRfc822Date(DateTime.Now));
            sReq.Headers.Add(LogConsts.NAME_HEADER_APIVERSION, LogConsts.VALUE_HEADER_APIVERSION);
            sReq.Headers.Add(LogConsts.NAME_HEADER_BODYRAWSIZE, "0");
            sReq.Headers.Add(LogConsts.NAME_HEADER_SIGMETHOD, LogConsts.VALUE_HEADER_SIGMETHOD);
            if (_securityToken != null && _securityToken.Length != 0)
            {
                sReq.Headers.Add(LogConsts.NAME_HEADER_ACS_SECURITY_TOKEN, _securityToken);
            }
        }

        private void FillCommonParameters(ServiceRequest sReq)
        {
            //TODO: add any additional parameters    
        }

        private Uri BuildReqEndpoint(LogRequest request)
        {
            //use empty string as project name if not set (expection will be thrown when do request)
            string project = request.IsSetProject() ? request.Project : String.Empty;
            return new Uri(this._uriScheme + "://" + project + "." + this._hostName + ":" + this._port);
        }
    }
}
