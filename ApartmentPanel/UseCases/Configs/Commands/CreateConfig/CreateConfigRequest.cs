using MediatR;

namespace ApartmentPanel.UseCases.Configs.Commands.CreateConfig
{
    public class CreateConfigRequest : IRequest<bool>
    {
        public CreateConfigRequest(string configName) => ConfigName = configName;

        public string ConfigName { get; set; }
    }
}
