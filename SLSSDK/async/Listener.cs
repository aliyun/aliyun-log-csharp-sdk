namespace Aliyun.Api.LOG.async
{
    public interface Listener<in T>
    {
        void onResponded(T response);
    }
}