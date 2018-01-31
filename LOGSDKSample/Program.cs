using Aliyun.Api.LOG.Common.Utilities;
using Aliyun.Api.LOG.Data;
using Aliyun.Api.LOG.Request;
using Aliyun.Api.LOG.Response;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Aliyun.Api.LOG.sample
{
    class LoghubSample
    {
        static void Main(string[] args)
        {
            // select you endpoint https://help.aliyun.com/document_detail/29008.html
            String endpoint = "your project region endpoint",
                accesskeyId = "your accesskey id",
                accessKey = "your access key",
                project = "",
                logstore = "";
            LogClient client = new LogClient(endpoint, accesskeyId, accessKey);
            //init http connection timeout
            client.ConnectionTimeout = client.ReadWriteTimeout = 10000;
            //list logstores
            foreach (String l in client.ListLogstores(new ListLogstoresRequest(project)).Logstores)
            {
                Console.WriteLine(l);
            }
            //put logs
            PutLogsRequest putLogsReqError = new PutLogsRequest();
            putLogsReqError.Project = project;
            putLogsReqError.Topic = "dotnet_topic";
            putLogsReqError.Logstore = logstore;
            putLogsReqError.LogItems = new List<LogItem>();
            for (int i = 1; i <= 10; ++i)
            {
                LogItem logItem = new LogItem();
                logItem.Time = DateUtils.TimeSpan();
                for (int k = 0; k < 10; ++k)
                    logItem.PushBack("error_" + i.ToString(), "invalid operation");
                putLogsReqError.LogItems.Add(logItem);
            }
            PutLogsResponse putLogRespError = client.PutLogs(putLogsReqError);

            Thread.Sleep(5000);

            //query logs, if query string is "", it means query all data
            GetLogsRequest getLogReq = new GetLogsRequest(project,
                logstore,
                DateUtils.TimeSpan() - 100,
                DateUtils.TimeSpan(),
                "dotnet_topic",
                "",
                100,
                0,
                false);
            GetLogsResponse getLogResp = client.GetLogs(getLogReq);
            Console.WriteLine("Log count : " + getLogResp.Logs.Count.ToString());
            for (int i = 0; i < getLogResp.Logs.Count; ++i)
            {
                var log = getLogResp.Logs[i];
                Console.WriteLine("Log time : " + DateUtils.GetDateTime(log.Time));
                for (int j = 0; j < log.Contents.Count; ++j)
                {
                    Console.WriteLine("\t" + log.Contents[j].Key + " : " + log.Contents[j].Value);
                }
                Console.WriteLine("");
            }

            //query histogram
            GetHistogramsResponse getHisResp = client.GetHistograms(new GetHistogramsRequest(project, 
                logstore, 
                DateUtils.TimeSpan() - 100, 
                DateUtils.TimeSpan(),
                "dotnet_topic",
                ""));
            Console.WriteLine("Histograms total count : " + getHisResp.TotalCount.ToString());

            //list shards
            ListShardsResponse listResp = client.ListShards(new ListShardsRequest(project, logstore));
            Console.WriteLine("Shards count : " + listResp.Shards.Count.ToString());

            //batch get logs
            for (int i = 0; i < listResp.Shards.Count; ++i)
            {
                //get cursor
                String cursor = client.GetCursor(new GetCursorRequest(project, logstore, listResp.Shards[i], ShardCursorMode.BEGIN)).Cursor;
                Console.WriteLine("Cursor : " + cursor);
                BatchGetLogsResponse batchGetResp = client.BatchGetLogs(new BatchGetLogsRequest(project, logstore, listResp.Shards[i], cursor, 10));
                Console.WriteLine("Batch get log, shard id : " + listResp.Shards[i].ToString() + ", log count : " + batchGetResp.LogGroupList.LogGroupList_Count.ToString());
            }
        }
    }
}