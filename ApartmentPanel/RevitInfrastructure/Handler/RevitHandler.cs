using Autodesk.Revit.UI;
using System;
using ApartmentPanel.RevitInfrastructure.Handler.HandlerStates;

namespace ApartmentPanel.RevitInfrastructure.Handler
{
    public class RevitHandler<TParameter, TResult>
    {
        private HandlerState<TParameter, TResult> _state;

        public TResult Execute(TParameter parameter)
        {
            try
            {
                TResult result = _state is null ? default : _state.Handle(parameter);
                return result;
            }
            catch (Exception ex)
            {
                TaskDialog.Show($"Exception", ex.Message);
                return default;
            }
        }
        internal void SetState(HandlerState<TParameter, TResult> state)
        {
            _state = state;
            _state.Handler = this;
        }
    }
}
