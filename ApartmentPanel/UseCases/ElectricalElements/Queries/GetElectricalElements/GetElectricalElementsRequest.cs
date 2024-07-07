using ApartmentPanel.UseCases.ApartmentElements.Dto;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using MediatR;
using System.Collections.Generic;

namespace ApartmentPanel.UseCases.ElectricalElements.Queries.GetElectricalElements
{
    public class GetElectricalElementsRequest : IRequest<ICollection<ElectricalElementDto>>
    {
    }
}
