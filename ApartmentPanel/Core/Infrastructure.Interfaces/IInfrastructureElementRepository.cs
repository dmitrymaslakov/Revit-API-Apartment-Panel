using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ApartmentPanel.Core.Infrastructure.Interfaces
{
    public interface IInfrastructureElementRepository
    {
        void AddToApartment(Action<List<(string name, string category)>> addElementsToApartment);
        //List<(string name, string category)> GetPropertiesByCategory(List<string> categories);
        void InsertToModel(Dictionary<string, string> elementDto);
    }
}