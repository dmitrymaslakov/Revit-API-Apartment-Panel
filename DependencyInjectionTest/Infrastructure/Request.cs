using System.Threading;

namespace DependencyInjectionTest.Infrastructure
{
    public enum RequestId : int
    {
        None,
        Insert,
        Configure,
        AddElement,
        EditElement,
        RemoveElement,
        AddCircuit,
        EditCircuit,
        RemoveCircuit
    }
    public class Request
    {
        private int _request = (int)RequestId.None;

        public RequestId Take()
        {
            return (RequestId)Interlocked.Exchange(ref _request, (int)RequestId.None);
        }
        public void Make(RequestId request)
        {
            Interlocked.Exchange(ref _request, (int)request);
        }

    }
}
