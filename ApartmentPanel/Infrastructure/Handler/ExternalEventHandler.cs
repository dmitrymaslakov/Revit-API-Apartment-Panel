using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using ApartmentPanel.Infrastructure.Handler.HandlerStates;

namespace ApartmentPanel.Infrastructure.Handler
{
    public class ExternalEventHandler : IExternalEventHandler
    {
        private HandlerState _state;

        public object Props { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                _state?.Handle(uiapp);
            }
            catch (Exception ex)
            {
                TaskDialog.Show($"Exception", ex.Message);
            }
        }
        public string GetName() => "Placement Apartment elements";
        internal void SetState(HandlerState state)
        {
            _state = state;
            _state.Handler = this;
        }
    }

    class LightingDeviceSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            // Only allow elements in the Lighting Devices category
            return elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_LightingDevices;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            // Not used in this example
            return false;
        }
    }
}
