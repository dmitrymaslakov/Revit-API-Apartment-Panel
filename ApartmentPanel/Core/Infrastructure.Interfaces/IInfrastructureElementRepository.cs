using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ApartmentPanel.Core.Infrastructure.Interfaces
{
    public interface IInfrastructureElementRepository
    {
        void AddToApartment(Action<IApartmentElement> addElementToApartment);
        void InsertToModel(Dictionary<string, string> elementDto);
    }
}