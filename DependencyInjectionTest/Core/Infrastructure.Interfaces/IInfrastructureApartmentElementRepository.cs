using DependencyInjectionTest.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace DependencyInjectionTest.Core.Infrastructure.Interfaces
{
    public interface IInfrastructureApartmentElementRepository
    {
        void AddToApartment(Action<IApartmentElement> addElementToApartment);
        void InsertElement(Dictionary<string, string> apartmentElementDto);
    }
}