using ApartmentPanel.Utility;
using Autodesk.Revit.UI;

namespace ApartmentPanel.RevitInfrastructure.Handler.HandlerStates
{
    internal abstract class HandlerState<TParameter, TResult> 
        : RevitInfrastructureBase
    {
        protected RevitHandler<TParameter, TResult> _handler;
        internal RevitHandler<TParameter, TResult> Handler { set => _handler = value; }
        public HandlerState(UIApplication uiapp) : base(uiapp) { }
        internal abstract TResult Handle(TParameter parameter);
    }
}
