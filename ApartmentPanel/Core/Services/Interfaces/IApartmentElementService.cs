using System.Collections.Generic;

namespace ApartmentPanel.Core.Services.Interfaces
{
    public interface IApartmentElementService
    {
        void Insert(Dictionary<string, string> apartmentElementDto);
    }
}