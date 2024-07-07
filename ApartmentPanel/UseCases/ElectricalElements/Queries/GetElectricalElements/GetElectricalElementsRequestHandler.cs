using ApartmentPanel.Infrastructure.Interfaces.Services;
using ApartmentPanel.UseCases.ApartmentElements.Dto;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.ElectricalElements.Queries.GetElectricalElements
{
    public class GetElectricalElementsRequestHandler
        : IRequestHandler<GetElectricalElementsRequest, ICollection<ElectricalElementDto>>
    {
        private readonly ICadServices _cadServices;
        public GetElectricalElementsRequestHandler(ICadServices cadServices)
        {
            _cadServices = cadServices;
        }
        public async Task<ICollection<ElectricalElementDto>> Handle(GetElectricalElementsRequest request, CancellationToken cancellationToken)
        {
            return await _cadServices.GetElectricalElementsAsync();
        }
    }
}
