using System;

namespace Aliyun.Api.LOG.async
{
    public class StdOutListener<T> : Listener<T>
    {
        public void onResponded(T response)
        {
            Console.WriteLine(response);
        }
    }
}