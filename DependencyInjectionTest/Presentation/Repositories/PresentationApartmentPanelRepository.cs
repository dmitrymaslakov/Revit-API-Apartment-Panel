using DependencyInjectionTest.Core.Presentation.Interfaces;
using DependencyInjectionTest.Presentation.View.Components;
using DependencyInjectionTest.Presentation.ViewModel.Interfaces;
using System.Linq;

namespace DependencyInjectionTest.Infrastructure.Repositories
{
    public class PresentationApartmentPanelRepository : IPresentationApartmentPanelRepository
    {
        private readonly IConfigPanelViewModel _configPanelViewModel;

        public PresentationApartmentPanelRepository(IConfigPanelViewModel configPanelViewModel) 
            => _configPanelViewModel = configPanelViewModel;

        public void ConfigurePanel() => new ConfigPanel(_configPanelViewModel).Show();
    }
}
