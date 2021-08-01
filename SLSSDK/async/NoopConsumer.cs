namespace Aliyun.Api.LOG.async
{
    public class NoopConsumer<T> : DataConsumer<T>
    {
        public void consumer(T data)
        {
        }
    }
}