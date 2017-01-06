#define __FT_TEST__

using Aliyun.Api.LOG;
using Aliyun.Api.LOG.Common.Communication;
using Aliyun.Api.LOG.Common.Utilities;
using Aliyun.Api.LOG.Data;
using Aliyun.Api.LOG.Utilities;
using Aliyun.Api.LOG.Request;
using Aliyun.Api.LOG.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace LOGSDKTest
{
    [TestClass]
    public class LogClientTest
    {
        private String RequestUri;
        private IDictionary<String, String> Headers = new Dictionary<String, String>();

        [TestMethod]
        public void TestException()
        {
            LogClient client = new LogClient(LogClientTestData.TEST_ENDPOINT, LogClientTestData.TEST_ACCESSKEYID, LogClientTestData.TEST_ACCESSKEY);
            client.SetWebSend(MockSend);
            //server error
            try
            {
                GetLogsRequest request = new GetLogsRequest();
                request.Project = LogClientTestData.TEST_PROJECT;
                request.Logstore = "server_error_return";
                request.Query = "error";
                request.Topic = "mockTopic";
                request.From = 1000;
                request.To = 2000;
                request.Lines = 200;
                request.Offset = 0;
                GetLogsResponse response = null;
                response = client.GetLogs(request);
                Assert.Fail("server exception is not triggered");
            }
            catch (LogException exp)
            {
                Assert.IsTrue(exp.ErrorCode.CompareTo("SLSServerErrorTest") == 0);
            }
            //json error
            try
            {
                GetLogsRequest request = new GetLogsRequest();
                request.Project = LogClientTestData.TEST_PROJECT;
                request.Logstore = "response_json_error_field";
                request.Query = "error";
                request.Topic = "mockTopic";
                request.From = 1000;
                request.To = 2000;
                request.Lines = 200;
                request.Offset = 0;
                GetLogsResponse response = null;
                response = client.GetLogs(request);
                Assert.Fail("json exception is not triggered");
            }
            catch (LogException exp)
            {
                Assert.IsTrue(exp.ErrorCode.CompareTo("SLSBadResponse") == 0);
            }
            try
            {
                GetLogsRequest request = new GetLogsRequest();
                request.Project = LogClientTestData.TEST_PROJECT;
                request.Logstore = "response_json_error_format";
                request.Query = "error";
                request.Topic = "mockTopic";
                request.From = 1000;
                request.To = 2000;
                request.Lines = 200;
                request.Offset = 0;
                GetLogsResponse response = null;
                response = client.GetLogs(request);
                Assert.Fail("json exception is not triggered");
            }
            catch (LogException exp)
            {
                Assert.IsTrue(exp.ErrorCode.CompareTo("SLSBadResponse") == 0);
            }
            //response stream error
            try
            {
                GetLogsRequest request = new GetLogsRequest();
                request.Project = LogClientTestData.TEST_PROJECT;
                request.Logstore = "response_stream_error";
                request.Query = "error";
                request.Topic = "mockTopic";
                request.From = 1000;
                request.To = 2000;
                request.Lines = 200;
                request.Offset = 0;
                GetLogsResponse response = null;
                response = client.GetLogs(request);
                Assert.Fail("io exception is not triggered");
            }
            catch (LogException exp)
            {
                Assert.IsTrue(exp.ErrorCode.CompareTo("SLSBadResponse") == 0);
            }
            
        }

        [TestMethod]
        public void TestUrlEncode()
        {
            Assert.IsTrue(HttpUtils.UrlEncode(LogClientTestData.TEST_URLENCODE.Key).CompareTo(LogClientTestData.TEST_URLENCODE.Value) == 0);
        }

        [TestMethod]
        public void TestDateOperation()
        {
            Assert.IsTrue(DateUtils.FormatRfc822Date(LogClientTestData.TEST_DATEPARSER.Key).CompareTo(LogClientTestData.TEST_DATEPARSER.Value) == 0);
        }

        [TestMethod]
        public void TestCalcMD5() 
        {
            Assert.IsTrue(LogClientTools.GetMd5Value(LogClientTestData.TEST_COMPUTMD5.Key).CompareTo(LogClientTestData.TEST_COMPUTMD5.Value) == 0);
        }

        [TestMethod]
        public void TestCalcSignature() 
        {
            String signature = LogClientTools.SigInternal(LogClientTestData.TEST_SIGNATURE.Key, LogClientTestData.TEST_ACCESSKEYID,LogClientTestData.TEST_ACCESSKEY);
            Assert.IsTrue(signature.CompareTo(LogClientTestData.TEST_SIGNATURE.Value) == 0);
        }

        [TestMethod]
        public void TestPutData() 
        {
            LogClient client = new LogClient(LogClientTestData.TEST_ENDPOINT, LogClientTestData.TEST_ACCESSKEYID, LogClientTestData.TEST_ACCESSKEY);
            client.SetWebSend(MockSend);
            LogGroup.Builder lgBuilder = LogGroup.CreateBuilder();
            lgBuilder.Topic = "testTopic";
            lgBuilder.Source = "127.0.0.1";
            Aliyun.Api.LOG.Log.Builder logBuilder = Aliyun.Api.LOG.Log.CreateBuilder();
            logBuilder.Time = 1000;
            Aliyun.Api.LOG.Log.Types.Content.Builder contentBuilder = Aliyun.Api.LOG.Log.Types.Content.CreateBuilder();
            contentBuilder.Key = "mockKey";
            contentBuilder.Value = "mockValue";
            logBuilder.AddContents(contentBuilder);
            lgBuilder.AddLogs(logBuilder);
            LogGroup tmpLogGroup = lgBuilder.Build();
            client.PutLogs(new PutLogsRequest(LogClientTestData.TEST_PROJECT, "testlogstore"), tmpLogGroup);
            Assert.IsTrue(DicToString(Headers).CompareTo("[x-sls-apiversion:0.4.0][x-sls-bodyrawsize:49][x-sls-signaturemethod:hmac-sha1][Content-Type:application/x-protobuf][x-sls-compresstype:deflate][Content-MD5:3D1836DDC6585AD78B60CCAE90C075DF][User-Agent:aliyun-sdk-dotnet/1.0.0.0]") == 0);
            //Assert.IsTrue(Host.CompareTo("mock_project.mockhost.aliyuncs.com") == 0);
            Assert.IsTrue(RequestUri.CompareTo("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore") == 0);
        }

        [TestMethod]
        public void TestLogClientWithRawIP()
        {
            try
            {
                LogClient client = new LogClient(LogClientTestData.TEST_IP_ENDPOINT, LogClientTestData.TEST_ACCESSKEYID, LogClientTestData.TEST_ACCESSKEY);
                Assert.Fail("sls doesn't support id address");
            }
            catch (LogException ex) {
                Assert.IsTrue(ex.ErrorCode.CompareTo("LogClientError") == 0);
            }
        }

        [TestMethod]
        public void TestGetLogs()
        {
            LogClient client = new LogClient(LogClientTestData.TEST_ENDPOINT, LogClientTestData.TEST_ACCESSKEYID, LogClientTestData.TEST_ACCESSKEY);
            client.SetWebSend(MockSend);
            GetLogsRequest request = new GetLogsRequest();
            request.Project = LogClientTestData.TEST_PROJECT;
            request.Logstore = "testlogstore";
            request.Query = "error";
            request.Topic = "mockTopic";
            request.From = 1000;
            request.To = 2000;
            request.Lines = 200;
            request.Offset = 0;
            GetLogsResponse response = client.GetLogs(request);
            Assert.IsTrue(DicToString(Headers).CompareTo("[x-sls-apiversion:0.4.0][x-sls-bodyrawsize:0][x-sls-signaturemethod:hmac-sha1][User-Agent:aliyun-sdk-dotnet/1.0.0.0]") == 0);
            Assert.IsTrue(response != null && response.Count == 2);
            //Assert.IsTrue(Host.CompareTo("mock_project.mockhost.aliyuncs.com") == 0);
            Assert.IsTrue(RequestUri.CompareTo("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore?type=log&topic=mockTopic&from=1000&to=2000&query=error&line=200&offset=0") == 0);
        }

        [TestMethod]
        public void TestListLogstore()
        {
            LogClient client = new LogClient(LogClientTestData.TEST_ENDPOINT, LogClientTestData.TEST_ACCESSKEYID, LogClientTestData.TEST_ACCESSKEY);
            client.SetWebSend(MockSend);
            ListLogstoresRequest request = new ListLogstoresRequest();
            request.Project = LogClientTestData.TEST_PROJECT;
            ListLogstoresResponse response = client.ListLogstores(request);
            Assert.IsTrue(DicToString(Headers).CompareTo("[x-sls-apiversion:0.4.0][x-sls-bodyrawsize:0][x-sls-signaturemethod:hmac-sha1][User-Agent:aliyun-sdk-dotnet/1.0.0.0]") == 0);
            Assert.IsTrue(response != null && response.Count == 3);
            //Assert.IsTrue(Host.CompareTo("mock_project.mockhost.aliyuncs.com") == 0);
            Assert.IsTrue(RequestUri.CompareTo("http://mock_project.mockhost.aliyuncs.com/logstores") == 0);
        }

        [TestMethod]
        public void TestGetHistograms()
        {
            LogClient client = new LogClient(LogClientTestData.TEST_ENDPOINT, LogClientTestData.TEST_ACCESSKEYID, LogClientTestData.TEST_ACCESSKEY);
            client.SetWebSend(MockSend);
            GetHistogramsRequest request = new GetHistogramsRequest();
            request.Project = LogClientTestData.TEST_PROJECT;
            request.Logstore = "testlogstore";
            request.Query = "error";
            request.Topic = "mockTopic";
            request.From = 1000;
            request.To = 2000;
            GetHistogramsResponse response = client.GetHistograms(request);
            Assert.IsTrue(DicToString(Headers).CompareTo("[x-sls-apiversion:0.4.0][x-sls-bodyrawsize:0][x-sls-signaturemethod:hmac-sha1][User-Agent:aliyun-sdk-dotnet/1.0.0.0]") == 0);
            Assert.IsTrue(response != null && response.TotalCount == 2);
            //Assert.IsTrue(Host.CompareTo("mock_project.mockhost.aliyuncs.com") == 0);
            Assert.IsTrue(RequestUri.CompareTo("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore?type=histogram&topic=mockTopic&from=1000&to=2000&query=error") == 0);
        }

        [TestMethod]
        public void TestListTopics()
        {
            LogClient client = new LogClient(LogClientTestData.TEST_ENDPOINT, LogClientTestData.TEST_ACCESSKEYID, LogClientTestData.TEST_ACCESSKEY);
            client.SetWebSend(MockSend);
            ListTopicsRequest request = new ListTopicsRequest();
            request.Project = LogClientTestData.TEST_PROJECT;
            request.Logstore = "testlogstore";
            request.Token = "atoken";
            request.Lines = 100;
            ListTopicsResponse response = client.ListTopics(request);
            Assert.IsTrue(DicToString(Headers).CompareTo("[x-sls-apiversion:0.4.0][x-sls-bodyrawsize:0][x-sls-signaturemethod:hmac-sha1][User-Agent:aliyun-sdk-dotnet/1.0.0.0]") == 0);
            Assert.IsTrue(response != null && response.Count == 2);
            //Assert.IsTrue(Host.CompareTo("mock_project.mockhost.aliyuncs.com") == 0);
            Assert.IsTrue(RequestUri.CompareTo("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore?type=topic&token=atoken&line=100") == 0);
        }

        [TestMethod]
        public void TestListTopicWithoutNextToken()
        {
            LogClient client = new LogClient(LogClientTestData.TEST_ENDPOINT, LogClientTestData.TEST_ACCESSKEYID, LogClientTestData.TEST_ACCESSKEY);
            client.SetWebSend(MockSend);
            ListTopicsRequest request = new ListTopicsRequest();
            request.Project = LogClientTestData.TEST_PROJECT;
            request.Logstore = "testlogstore_without_nexttoken";
            request.Token = "atoken";
            request.Lines = 100;
            ListTopicsResponse response = client.ListTopics(request); ;
            Assert.IsTrue(DicToString(Headers).CompareTo("[x-sls-apiversion:0.4.0][x-sls-bodyrawsize:0][x-sls-signaturemethod:hmac-sha1][User-Agent:aliyun-sdk-dotnet/1.0.0.0]") == 0);
            Assert.IsTrue(response != null && response.Count == 2);
            //Assert.IsTrue(Host.CompareTo("mock_project.mockhost.aliyuncs.com") == 0);
            Assert.IsTrue(RequestUri.CompareTo("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore_without_nexttoken?type=topic&token=atoken&line=100") == 0);
        }

        
        [TestMethod]
        public void FT()
        {
            LogClient client = new LogClient("sls-failover.alibaba-inc.com", "", "");
            uint topicFlag = DateUtils.TimeSpan();
            int PUT_COUNT = 20, TOPIC_COUNT = 10, LOGITEM_COUNT = 20, CONTENT_COUNT = 10, SLEEP_INTERVAL = 2, SLEEP_TIME = 500;

            for (int j = 1; j <= PUT_COUNT; ++j)
            {
                PutLogsRequest putLogsReqError = new PutLogsRequest();
                putLogsReqError.Project = "ali-winlogtail-project";
                putLogsReqError.Topic = "dotnet_topic_" + topicFlag + "_" + (j % TOPIC_COUNT);
                putLogsReqError.Logstore = "sls-logstore-002";
                putLogsReqError.LogItems = new List<LogItem>();
                for (int i = 1; i <= LOGITEM_COUNT; ++i)
                {
                    LogItem logItem = new LogItem();
                    logItem.Time = (uint)(topicFlag + j);
                    for (int k = 0; k < CONTENT_COUNT; ++k)
                        logItem.PushBack("error_" + (j % TOPIC_COUNT) + "_" + k, "invalid operation: " + i * j);
                    putLogsReqError.LogItems.Add(logItem);
                }
                PutLogsResponse putLogRespError = client.PutLogs(putLogsReqError);
                if (j % SLEEP_INTERVAL == 0)
                    Thread.Sleep(SLEEP_TIME);
            }
            Thread.Sleep(50 * 1000);

            ListLogstoresRequest req = new ListLogstoresRequest();
            req.Project = "ali-winlogtail-project";
            ListLogstoresResponse res = client.ListLogstores(req);
            HashSet<String> logstoresSet = new HashSet<string>(res.Logstores);
            Assert.IsTrue(logstoresSet.Contains("sls-logstore-002"));
            

            ListTopicsRequest topicReq = new ListTopicsRequest();
            topicReq.Project = "ali-winlogtail-project";
            topicReq.Logstore = "sls-logstore-002";
            topicReq.Lines = TOPIC_COUNT;
            topicReq.Token = "dotnet_topic_" + topicFlag + "_";
            ListTopicsResponse lstTopicsRequest = client.ListTopics(topicReq);
            Assert.IsTrue(lstTopicsRequest.Count >= TOPIC_COUNT);
            HashSet<String> topicSet = new HashSet<string>(lstTopicsRequest.Topics);
            for (int i = 0; i < TOPIC_COUNT; ++i)
                Assert.IsTrue(topicSet.Contains("dotnet_topic_" + topicFlag + "_" + i));
            Thread.Sleep(SLEEP_TIME);
            for (int i = 0; i < TOPIC_COUNT; ++i)
            {
                GetHistogramsRequest histReq = new GetHistogramsRequest();
                histReq.Project = "ali-winlogtail-project";
                histReq.Logstore = "sls-logstore-002";
                histReq.Topic = "dotnet_topic_" + topicFlag + "_" + i;
                histReq.To = (uint)(topicFlag + PUT_COUNT + 1);
                histReq.From = (uint)topicFlag;
                GetHistogramsResponse histResp = client.GetHistograms(histReq);
                Assert.IsTrue(histResp.TotalCount == (PUT_COUNT / TOPIC_COUNT) * LOGITEM_COUNT);
                if ((i + 1) % SLEEP_INTERVAL == 0)
                    Thread.Sleep(SLEEP_TIME);
            }
            Thread.Sleep(SLEEP_TIME);
            for (int i = 0; i < TOPIC_COUNT; ++i)
                for (int k = 0; k < 2; ++k)
                {
                    GetHistogramsRequest histReq = new GetHistogramsRequest();
                    histReq.Project = "ali-winlogtail-project";
                    histReq.Logstore = "sls-logstore-002";
                    histReq.Topic = "dotnet_topic_" + topicFlag + "_" + i;
                    histReq.Query = "error_" + i + "_" + k;
                    histReq.To = (uint)(topicFlag + PUT_COUNT + 1);
                    histReq.From = (uint)topicFlag;
                    GetHistogramsResponse histResp = client.GetHistograms(histReq);
                    Assert.IsTrue(histResp.TotalCount == (PUT_COUNT / TOPIC_COUNT) * LOGITEM_COUNT);
                    if ((k +1)*(i + 1) % SLEEP_INTERVAL == 0)
                        Thread.Sleep(SLEEP_TIME);
                }
            Thread.Sleep(SLEEP_TIME);
            for (int i = 0; i < TOPIC_COUNT; ++i)
            {
                GetLogsRequest getLogsReq = new GetLogsRequest();
                getLogsReq.Project = "ali-winlogtail-project";
                getLogsReq.Logstore = "sls-logstore-002";
                getLogsReq.Topic = "dotnet_topic_" + topicFlag + "_" + i;
                getLogsReq.Lines = 120;
                getLogsReq.To = (uint)(topicFlag + PUT_COUNT + 1);
                getLogsReq.From = (uint)topicFlag;
                GetLogsResponse getLogsResp = client.GetLogs(getLogsReq);
                Assert.IsTrue(getLogsResp.Count == (PUT_COUNT / TOPIC_COUNT) * LOGITEM_COUNT);
                String logs = getLogsResp.Print();
                for (int m = 0; m < CONTENT_COUNT; ++m)
                {
                    String dstStr = "error_" + i + "_" + m;
                    Assert.IsTrue(ChildStringOccurTimes(logs, dstStr) == getLogsResp.Count);
                }
                if ((i + 1) % SLEEP_INTERVAL == 0)
                    Thread.Sleep(SLEEP_TIME);
            }
            Thread.Sleep(SLEEP_TIME);
            for (int i = 0; i < TOPIC_COUNT; ++i)
                for (int k = 0; k < 2; ++k)
                {
                    GetLogsRequest getLogsReq = new GetLogsRequest();
                    getLogsReq.Project = "ali-winlogtail-project";
                    getLogsReq.Logstore = "sls-logstore-002";
                    getLogsReq.Topic = "dotnet_topic_" + topicFlag + "_" + i;
                    getLogsReq.Query = "error_" + i + "_" + k;
                    getLogsReq.Lines = 120;
                    getLogsReq.To = (uint)(topicFlag + PUT_COUNT + 1);
                    getLogsReq.From = (uint)topicFlag;
                    GetLogsResponse getLogsResp = client.GetLogs(getLogsReq);
                    Assert.IsTrue(getLogsResp.Count == (PUT_COUNT / TOPIC_COUNT) * LOGITEM_COUNT);
                    String logs = getLogsResp.Print();
                    for (int m = 0; m < CONTENT_COUNT; ++m)
                    {
                        String dstStr = "error_" + i + "_" + m;
                        Assert.IsTrue(ChildStringOccurTimes(logs, dstStr) == getLogsResp.Count);
                    }
                    if ((k + 1) * (i + 1) % SLEEP_INTERVAL == 0)
                        Thread.Sleep(SLEEP_TIME);
                }
            Console.WriteLine();
        }
        private Aliyun.Api.LOG.Common.Communication.ServiceClientImpl.ResponseImpl MockSend(HttpWebRequest request)
        {
            RequestUri = request.RequestUri.ToString();
            //not supported by.Net35 
            //Host = request.Host;
            Headers.Clear();
            string[] headers = request.Headers.AllKeys;

            foreach (string s in headers)
            {
                Headers.Add(s, request.Headers.Get(s));
            }

            Aliyun.Api.LOG.Common.Communication.ServiceClientImpl.ResponseImpl response = new Aliyun.Api.LOG.Common.Communication.ServiceClientImpl.ResponseImpl();

            if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore?type=histogram"))
            {
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(LogClientTestData.JSON_GETSTATUS));
            }
            else if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore?type=topic"))
            {
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(LogClientTestData.JSON_LISTTOPICS));
            }
            else if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore_without_nexttoken?type=topic"))
            {
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(LogClientTestData.JSON_LISTTOPICS_WITHOUT_NEXTTOKEN));
            }
            else if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore"))
            {
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(LogClientTestData.JSON_GETLOGS));
            }
            else if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores/server_error_return"))
            {
#if(!__FT_TEST__)
                response.StatusCode = HttpStatusCode.NotAcceptable;
#endif
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes("{\"error_code\":\"SLSServerErrorTest\",\"error_message\":\"server error\"}"));
            }
            else if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores/response_json_error_field"))
            {
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(LogClientTestData.ERROR_JSON_GETLOGS_FIELD));
            }
            else if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores/response_json_error_format"))
            {
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(LogClientTestData.ERROR_JSON_GETLOGS_FORMAT));
            }
            else if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores/response_stream_error"))
            {
                response.Content = new CannotReadStream();
            }
            else if (request.Address.AbsoluteUri.ToString().StartsWith("http://mock_project.mockhost.aliyuncs.com/logstores"))
            {
                response.Content = new MemoryStream(Encoding.UTF8.GetBytes(LogClientTestData.JSON_LISTLOGSTORES));
            }

            return response;
        }

        private static String DicToString(IDictionary<String, String> Headers)
        {
            if (Headers == null || Headers.Count == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<String, String> pair in Headers)
            {
                if (pair.Key.CompareTo(LogConsts.NAME_HEADER_DATE) != 0 && pair.Key.CompareTo(LogConsts.NAME_HEADER_AUTH) != 0)
                    sb.Append("[" + pair.Key + ":" + pair.Value + "]");
            }
            return sb.ToString();
        }

        private static int ChildStringOccurTimes(String src, String dst,int start = 0) 
        {
            int index = -1;
            if ((index = src.IndexOf(dst, start)) < 0)
                return 0;
            return 1 + ChildStringOccurTimes(src, dst, index + dst.Length);
        }
    }
}
