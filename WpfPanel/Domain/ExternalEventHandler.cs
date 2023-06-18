using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPanel.Domain
{
    internal class ExternalEventHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app) => RequestHandler.Execute(app, RequestId.Insert);

        public string GetName() => "InsertTriss";
    }
}
