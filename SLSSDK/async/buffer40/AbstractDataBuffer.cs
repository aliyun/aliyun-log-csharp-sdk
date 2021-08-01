using System;
using System.Collections.Concurrent;

namespace Aliyun.Api.LOG.async
{
    public abstract class AbstractDataBuffer<T> : DataBuffer<T>
    {
        private const int DEFAULT_SAVE_TIMEOUT = 10;
        private const int DEFAULT_FETCH_DATA_TIMEOUT = 10;
        private readonly BlockingCollection<T>[] _buffer;

        protected delegate T fetchDataType();

        protected AbstractDataBuffer(int line, int length)
        {
            _buffer = InitBuffer(line, length);
        }

        private static BlockingCollection<T>[] InitBuffer(int line, int length)
        {
            var buffer = new BlockingCollection<T>[line];

            for (var i = 0; i < line; i++)
            {
                buffer[i] = new BlockingCollection<T>(length);
            }

            return buffer;
        }

        public void save(T data)
        {
            if (BlockingCollection<T>.TryAddToAny(_buffer, data, DEFAULT_SAVE_TIMEOUT) == -1)
            {
                throw new BufferFullException();
            }
        }

        protected T TryTake()
        {
            try
            {
                BlockingCollection<T>.TryTakeFromAny(_buffer, out var data, DEFAULT_FETCH_DATA_TIMEOUT);
                return data;
            }
            catch (Exception)
            {
                return default;
            }
        }

        protected T Take()
        {
            try
            {
                BlockingCollection<T>.TakeFromAny(_buffer, out var data);
                return data;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}