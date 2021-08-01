using System;
using System.Diagnostics;
using System.Threading;
using static System.Int32;

namespace Aliyun.Api.LOG.async
{
    public class NotifyDataBuffer<T> : AbstractDataBuffer<T>
    {
        private readonly int _interval;
        private readonly DataConsumer<T> _dataConsumer;
        private const int DEFAULT_LINE = 16;
        private const int DEFAULT_LENGHT = 16;

        private NotifyDataBuffer(int line, int length, int interval, DataConsumer<T> dataConsumer) : base(line,
            length)
        {
            registryConsumer(dataConsumer);
            if (interval > 0)
            {
                registryScheduleConsumer(dataConsumer, interval);
            }

            registryProcessExitHandler(dataConsumer);
        }

        private void registryConsumer(DataConsumer<T> dataConsumer)
        {
            new Thread(() =>
            {
                while (true)
                {
                    fetchData(dataConsumer, Take);
                }
            }) {IsBackground = true, Name = "NotifyDataBuffer Thread"}.Start();
        }

        private void registryProcessExitHandler(DataConsumer<T> dataConsumer)
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => { fetchData(dataConsumer, TryTake); };
        }

        private void registryScheduleConsumer(DataConsumer<T> dataConsumer, int interval)
        {
            new Thread(() =>
            {
                while (true)
                {
                    fetchData(dataConsumer, TryTake);
                    Thread.Sleep(interval);
                }
            }) {IsBackground = true, Name = "NotifyDataBuffer Schedule Thread"}.Start();
        }

        private static void fetchData(DataConsumer<T> dataConsumer, fetchDataType fetchData)
        {
            var data = fetchData();

            if (data != null)
            {
                dataConsumer.consumer(data);
            }
        }

        public class Builder<T>
        {
            private readonly int _line;
            private readonly int _length;
            private int _interval = MinValue;
            private DataConsumer<T> dataConsumer;

            private Builder(int line, int length)
            {
                _line = line;
                _length = length;
            }

            public NotifyDataBuffer<T> build()
            {
                dataConsumer = dataConsumer ?? new NoopConsumer<T>();
                return new NotifyDataBuffer<T>(_line, _length, _interval, dataConsumer);
            }

            public static Builder<T> newBuilder(int line = DEFAULT_LINE, int length = DEFAULT_LENGHT)
            {
                return new Builder<T>(line, length);
            }


            public Builder<T> withScheduleConsumer(int intervalMs)
            {
                _interval = intervalMs;
                return this;
            }

            public Builder<T> withConsumer(DataConsumer<T> dataConsumer)
            {
                this.dataConsumer = dataConsumer;
                return this;
            }
        }
    }
}