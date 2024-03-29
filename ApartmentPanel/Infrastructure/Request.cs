﻿using System.Threading;

namespace ApartmentPanel.Infrastructure
{
    public enum RequestId : int
    {
        None,
        Insert,
        BatchInsert,
        Configure,
        AddElement,
        EditElement,
        RemoveElement,
        AddCircuit,
        EditCircuit,
        RemoveCircuit,
        SettingParameters,
        GetProperties
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
