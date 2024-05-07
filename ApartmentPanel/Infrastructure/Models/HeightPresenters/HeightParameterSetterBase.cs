using ApartmentPanel.Presentation.Models;
using ApartmentPanel.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public abstract class HeightParameterSetterBase : RevitInfrastructureBase, IHeightParameterSetter
    {
        public HeightParameterSetterBase(UIApplication uiapp) : base(uiapp) { }
        public abstract void Set(Parameter parameter, double height);
    }
}
