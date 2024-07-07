using ApartmentPanel.Core.Models;
using ApartmentPanel.UseCases.ApartmentElements.Dto;
using MediatR;

namespace ApartmentPanel.UseCases.ApartmentElements.Commands.CreateApartmentElement
{
    public class CreateApartmentElementRequest : IRequest<ApartmentElement>
    {
        public CreateApartmentElementRequest(ApartmentElementCreateDto elementDto) 
            => ApartmentElementCreateDto = elementDto;

        public ApartmentElementCreateDto ApartmentElementCreateDto { get; set; }
    }
}
