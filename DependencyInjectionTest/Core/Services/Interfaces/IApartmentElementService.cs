using Autodesk.Revit.DB;
using DependencyInjectionTest.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace DependencyInjectionTest.Core.Services.Interfaces
{
    public interface IApartmentElementService
    {
        void Insert(Dictionary<string, string> apartmentElementDto);
        void AddToApartment(Action<IApartmentElement> addElementToApartment);
        void RemoveFromApartment();
        void AddToCircuit(IApartmentElement apartmentElement);
    }
}