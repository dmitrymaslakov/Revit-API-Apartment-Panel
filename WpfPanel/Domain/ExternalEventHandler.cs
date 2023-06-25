using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPanel.Domain
{
    internal class ExternalEventHandler
    {
        public void Execute() => RequestHandler.Execute(RequestId.Insert);

        public string GetName() => "InsertTriss";
    }
}
