using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LOGSDKTest
{
    internal class LogClientTestData
    {
        public static KeyValuePair<String, String> TEST_URLENCODE = new KeyValuePair<String, String>("slssdk0.4&#%$ @!*", "slssdk0.4%26%23%25%24%20%40%21%2A");
        public static KeyValuePair<DateTime, String> TEST_DATEPARSER = new KeyValuePair<DateTime, string>(new DateTime(2008, 3, 9, 16, 5, 7, 123), "Sun, 09 Mar 2008 08:05:07 GMT");
        public static KeyValuePair<byte[], String> TEST_COMPUTMD5 = new KeyValuePair<byte[], String>(new byte[] { 9, 8, 1, 0, 13, 19 }, "6E809AA357CAB045F134D9B67B259C4F");
        public static KeyValuePair<String, String> TEST_SIGNATURE = new KeyValuePair<String, String>("http://mock_project.mockhost.aliyuncs.com/logstores/testlogstore", "SLS mockkeyid:to3k5R5HMjZhBk8gO2sJRilcKFQ=");
        public static String TEST_ENDPOINT = "http://mockhost.aliyuncs.com";
        public static String TEST_IP_ENDPOINT = "http://192.168.1.101";
        public static String TEST_PROJECT = "mock_project";
        public static String TEST_ACCESSKEYID = "mockkeyid";
        public static String TEST_ACCESSKEY = "mockkey";
        public static String JSON_LISTLOGSTORES = "{ \"count\": 3, \"logstores\":[\"app_log\", \"access_log\", \"op_log\"] }";
        public static String JSON_LISTTOPICS = "{ \"count\": 2, \"next_token\":\"Topic2\", \"topics\": [\"Topic1\", \"Topic2\"] }";
        public static String JSON_LISTTOPICS_WITHOUT_NEXTTOKEN = "{ \"count\": 2, \"topics\": [\"Topic1\", \"Topic2\"] }";
        public static String JSON_GETSTATUS = "{\"progress\":\"Complete\",\"count\": 2,\"histograms\":[{\"from\":1459001650,\"to\":1459001690,\"count\":11234,\"progress\":\"Complete\"},{\"from\":1459001790,\"to\":1459001990,\"count\":11234,\"progress\":\"Complete\"}]}";
        public static String JSON_GETLOGS = "{\"count\":2,\"progress\":\"Complete\",\"logs\":[{\"__time__\" : 1450900861, \"__source__\" : \"10.237.0.17\",\"Key1\" : \"Value1\",\"Key2\" : \"Value2\"},{\"__time__\" : 1450900862,  \"__source__\" : \"10.237.0.17\",\"Key1\" : \"Value1\",\"Key2\" : \"Value2\"}]}";
        public static String ERROR_JSON_GETLOGS_FIELD = "{\"total\":2,\"progress\":\"Complete\",\"logs\":[{\"__time__\" : 1450900861, \"__source__\" : \"10.237.0.17\",\"Key1\" : \"Value1\",\"Key2\" : \"Value2\"},{\"__time__\" : 1450900862,  \"__source__\" : \"10.237.0.17\",\"Key1\" : \"Value1\",\"Key2\" : \"Value2\"}]}";
        public static String ERROR_JSON_GETLOGS_FORMAT = "{\"count\":2,\"progress\":\"Complete\",\"logs\":{{\"__time__\" : 1450900861, \"__source__\" : \"10.237.0.17\",\"Key1\" : \"Value1\",\"Key2\" : \"Value2\"},{\"__time__\" : 1450900862,  \"__source__\" : \"10.237.0.17\",\"Key1\" : \"Value1\",\"Key2\" : \"Value2\"}]}";
    }
    internal class CannotReadStream : Stream
    {
        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
