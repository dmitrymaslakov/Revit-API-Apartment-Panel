using Autodesk.Revit.UI;

namespace ApartmentPanel.Core.Infrastructure.Interfaces
{
    public interface IApartmentElementRepository
    {
        void InsertElement(UIApplication Uiapp, object Props);
    }
}