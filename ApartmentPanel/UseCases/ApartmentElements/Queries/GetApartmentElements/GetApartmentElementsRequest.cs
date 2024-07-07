using ApartmentPanel.Core.Models;
using ApartmentPanel.UseCases.ApartmentElements.Dto;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using MediatR;
using System.Collections.Generic;

namespace ApartmentPanel.UseCases.ApartmentElements.Queries.GetApartmentElements
{
    public class GetApartmentElementsRequest : IRequest<ICollection<ElectricalElementDto>>
    {
    }
}
