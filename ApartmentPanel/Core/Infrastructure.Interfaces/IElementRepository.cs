using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using System;
using System.Collections.Generic;

namespace ApartmentPanel.Core.Infrastructure.Interfaces
{
    public interface IElementRepository
    {
        void AddToApartment(Action<List<(string name, string category)>> addElementsToApartment);
        void Analize();

        void InsertToModel(InsertElementDTO elementDto);
    }
}