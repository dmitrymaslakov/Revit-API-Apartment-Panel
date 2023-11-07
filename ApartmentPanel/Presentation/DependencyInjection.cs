using ApartmentPanel.Presentation.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApartmentPanel.Presentation.View;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Presentation.Services;

namespace ApartmentPanel.Presentation
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddPresentationServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services => 
            {
                services.AddSingleton<IListElementsViewModel, ListElementsViewModel>();
                services.AddSingleton<IConfigPanelViewModel, ConfigPanelViewModel>();
                services.AddSingleton<IMainViewModel, MainViewModel>();
                services.AddSingleton(provider => new MainView(provider.GetService<IMainViewModel>()));
                services.AddTransient<ModelAnalizing>();
                /*services.AddTransient<IPresentationElementRepository, PresentationElementRepository>();
                services.AddTransient<IPresentationPanelRepository, PresentationPanelRepository>();*/
            });
            return hostBuilder;
        }
    }
}
