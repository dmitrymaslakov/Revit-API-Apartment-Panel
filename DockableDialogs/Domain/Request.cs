using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DockableDialogs.Domain
{
    public enum RequestId : int
    {
        None,
        Insert,
        Test
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
