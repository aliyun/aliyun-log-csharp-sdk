using System;
using System.Threading;
using Aliyun.Api.LOG.Common.Communication;
using Aliyun.Api.LOG.Response;
using ExecutionContext = Aliyun.Api.LOG.Common.Communication.ExecutionContext;

namespace Aliyun.Api.LOG.async
{
    internal class PutLogsParameters
    {
        internal readonly ServiceRequest _sReq;
        internal readonly ExecutionContext _context;
        internal readonly PutLogsResponseListener _listener;
        internal int retryTimes;

        public PutLogsParameters(ServiceRequest sReq, ExecutionContext context, PutLogsResponseListener listener)
        {
            _sReq = sReq;
            _context = context;
            _listener = listener;
            retryTimes = 0;
        }
    }

    internal class PutLogsDataConsumer : DataConsumer<PutLogsParameters>
    {
        private readonly ServiceClient _serviceClient;
        private readonly int _maxRetryTime;
        private const int MAX_RETRY_TIMES = 7;

        public PutLogsDataConsumer(ServiceClient serviceClient, int retryTime = MAX_RETRY_TIMES)
        {
            _serviceClient = serviceClient;
            _maxRetryTime = retryTime;
        }

        private static int generateRetryTimes(int retryTime)
        {
            return (2 ^ retryTime) * 1000 + new Random().Next(1, 200);
        }

        void DataConsumer<PutLogsParameters>.consumer(PutLogsParameters parameters)
        {
            while (parameters.retryTimes < _maxRetryTime)
            {
                try
                {
                    using (var response = _serviceClient.Send(parameters._sReq, parameters._context))
                    {
                        if ((int) response.StatusCode < 500)
                        {
                            parameters._listener.onResponded(new PutLogsResponse(response.Headers));
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    // do nothing
                }

                Thread.Sleep(generateRetryTimes(++parameters.retryTimes));
            }
        }
    }
}