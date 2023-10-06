using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ApartmentPanel.Core.Infrastructure.Interfaces
{
    public interface IApartmentElementRepository
    {
        void InsertElement(Dictionary<string, string> apartmentElementDto);
    }
}