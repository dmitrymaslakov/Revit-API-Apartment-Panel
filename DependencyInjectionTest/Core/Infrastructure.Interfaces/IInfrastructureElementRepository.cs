using DependencyInjectionTest.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace DependencyInjectionTest.Core.Infrastructure.Interfaces
{
    public interface IInfrastructureElementRepository
    {
        void AddToApartment(Action<IApartmentElement> addElementToApartment);
        void InsertToModel(Dictionary<string, string> elementDto);
    }
}