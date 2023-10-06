using DependencyInjectionTest.Core.Services.Interfaces;

namespace DependencyInjectionTest.Presentation.Commands
{
    public abstract class BaseCommandsCreater
    {
        protected readonly IApartmentElementService _apartmentElementService;
        protected readonly IApartmentPanelService _apartmentPanelService;

        protected BaseCommandsCreater(IApartmentElementService apartmentElementService, 
            IApartmentPanelService apartmentPanelService)
        {
            _apartmentElementService = apartmentElementService;
            _apartmentPanelService = apartmentPanelService;
        }
    }
}
