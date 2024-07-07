using ApartmentPanel.Core.Services.Interfaces;

namespace ApartmentPanel.Presentation.Commands
{
    public abstract class BaseCommandsCreater
    {
        protected readonly IElementService _elementService;

        protected BaseCommandsCreater(IElementService elementService) => 
            _elementService = elementService;
        protected BaseCommandsCreater()
        {
            
        }
    }
}
