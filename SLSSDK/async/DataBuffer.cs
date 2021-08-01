namespace Aliyun.Api.LOG.async
{
    public interface DataBuffer<T>
    {
        void save(T data);
    }
}