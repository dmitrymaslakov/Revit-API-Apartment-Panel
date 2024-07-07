using ApartmentPanel.UseCases.Configs.Dto;
using MediatR;

namespace ApartmentPanel.UseCases.Configs.Queries.GetConfig
{
    public class GetConfigRequest : IRequest<GetConfigDto>
    {
        public GetConfigRequest(string configName) => ConfigName = configName;

        public string ConfigName { get; set; }
    }
}
