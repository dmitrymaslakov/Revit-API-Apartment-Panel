using ApartmentPanel.Utility;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Handler.HandlerStates
{
    internal abstract class HandlerState : RevitInfrastructureBase
    {
        protected ExternalEventHandler _handler;
        internal ExternalEventHandler Handler { set => _handler = value; }
        internal abstract void Handle(UIApplication uiapp);
    }
}
