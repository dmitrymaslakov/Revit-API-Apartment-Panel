using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Core.Services.Interfaces
{
    public interface IElementService
    {
        void InsertToModel(InsertElementDTO apartmentElementDto);
        JsonSerializerOptions GetSerializationOptions();
        List<IApartmentElement> GetAll(List<(string name, string category)> props);
        void AddToApartment(Action<List<(string name, string category)>> addElementToApartment);
        IApartmentElement CloneFrom(IApartmentElement element);
        void SetAnnotationTo(IApartmentElement element, BitmapSource img);
        ImageSource GetAnnotationFor(string elementName);
        void InsertBatchToModel(InsertBatchDTO batchDto);
        void SetBatchedElementParameters(SetParamsDTO setParamsDTO);
    }
}