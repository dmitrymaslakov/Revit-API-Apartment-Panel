using ApartmentPanel.Core.Models;
using ApartmentPanel.Infrastructure.Interfaces.Services;
using ApartmentPanel.UseCases.ApartmentElements.Dto;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.ApartmentElements.Queries.GetApartmentElements
{
    public class GetApartmentElementsRequestHandler
        : IRequestHandler<GetApartmentElementsRequest, ICollection<ElectricalElementDto>>
    {
        private readonly ICadServices _cadServices;
        //private readonly IMapper _mapper;

        public GetApartmentElementsRequestHandler(ICadServices cadServices)//, IMapper mapper)
        {
            _cadServices = cadServices;
            //_mapper = mapper;
        }
        public async Task<ICollection<ElectricalElementDto>> Handle(GetApartmentElementsRequest request, CancellationToken cancellationToken)
        {
            var electricalFamilies = await _cadServices.GetElectricalElementsAsync();
            /*var apartmentElement = electricalFamilies
                .Select(ef => _mapper.Map<ApartmentElement>(ef))
                .ToList();*/
            return electricalFamilies;
        }
    }
}
