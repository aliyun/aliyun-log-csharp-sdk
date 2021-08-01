namespace Aliyun.Api.LOG.async
{
    public class NoopListener<T> : Listener<T>
    {
        public void onResponded(T response)
        {
            // Do-Nothing
        }
    }
}