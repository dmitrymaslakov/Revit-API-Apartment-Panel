using ApartmentPanel.Core.Models;
using ApartmentPanel.UseCases.ApartmentElements.Dto;
using MediatR;

namespace ApartmentPanel.UseCases.ApartmentElements.Commands.UpdateApartmentElement
{
    public class UpdateApartmentElementRequest : IRequest<ApartmentElement>
    {
        public UpdateApartmentElementRequest(ApartmentElementUpdateDto dto) => Dto = dto;

        public ApartmentElementUpdateDto Dto { get; set; }
    }
}
