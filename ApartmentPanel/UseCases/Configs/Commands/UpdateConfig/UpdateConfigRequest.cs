using ApartmentPanel.UseCases.Configs.Dto;
using MediatR;

namespace ApartmentPanel.UseCases.Configs.Commands.UpdateConfig
{
    public class UpdateConfigRequest : IRequest
    {
        public UpdateConfigRequest(GetConfigDto dto) => ConfigDto = dto;

        public GetConfigDto ConfigDto { get; set; }
    }
}
