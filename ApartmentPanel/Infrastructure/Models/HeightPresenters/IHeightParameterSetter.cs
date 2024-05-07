using ApartmentPanel.Presentation.Models;
using Autodesk.Revit.DB;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public interface IHeightParameterSetter
    {
        void Set(Parameter parameter, double height);
    }
}
