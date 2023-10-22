using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ApartmentPanel.Core.Services.Interfaces
{
    public interface IElementService
    {
        void InsertToModel(Dictionary<string, string> apartmentElementDto);
        JsonSerializerOptions GetSerializationOptions();
        List<IApartmentElement> GetAll(List<(string name, string category)> props);
        void AddToApartment(Action<List<(string name, string category)>> addElementToApartment);
        //void AddToApartment(Action<IApartmentElement> addElementToApartment);
    }
}