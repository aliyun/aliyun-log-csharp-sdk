namespace Aliyun.Api.LOG.async
{
    public interface DataConsumer<in T>
    {
        void consumer(T data);
    }
}