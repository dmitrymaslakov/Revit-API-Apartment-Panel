using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ApartmentPanel.Core.Services.Interfaces
{
    public interface IElementService
    {
        void InsertToModel(Dictionary<string, string> apartmentElementDto);
        void AddToApartment(Action<IApartmentElement> addElementToApartment);
        JsonSerializerOptions GetSerializationOptions();
        //void AddToCircuit();
        //void RemoveFromApartment();
        //void RemoveFromCircuit();
    }
}