using ApartmentPanel.Core.Services.Interfaces;

namespace ApartmentPanel.Presentation.Commands
{
    public abstract class BaseCommandsCreater
    {
        protected readonly IElementService _elementService;
        protected readonly IPanelService _panelService;

        protected BaseCommandsCreater(IElementService elementService, 
            IPanelService panelService)
        {
            _elementService = elementService;
            _panelService = panelService;
        }
    }
}
