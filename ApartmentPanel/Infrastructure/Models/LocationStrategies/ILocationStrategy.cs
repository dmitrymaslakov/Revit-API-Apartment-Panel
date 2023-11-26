using ApartmentPanel.Presentation.Models;
using Autodesk.Revit.DB;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public interface ILocationStrategy
    {
        void SetRequiredLocation(FamilyInstance familyInstance, double height);
    }
}
