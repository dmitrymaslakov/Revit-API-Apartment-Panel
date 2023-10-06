using DependencyInjectionTest.Presentation.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DependencyInjectionTest.Presentation.View;
using DependencyInjectionTest.Presentation.ViewModel.Interfaces;
using DependencyInjectionTest.Infrastructure.Repositories;
using DependencyInjectionTest.Core.Presentation.Interfaces;
using DependencyInjectionTest.Presentation.ViewModel.ComponentsVM;

namespace DependencyInjectionTest.Presentation
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddPresentationServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services => 
            {
                services.AddSingleton<IConfigPanelViewModel, ConfigPanelViewModel>();
                services.AddSingleton<IMainViewModel, MainViewModel>();
                services.AddSingleton(provider => new MainView(provider.GetService<IMainViewModel>()));
                services.AddTransient<IPresentationApartmentElementRepository, PresentationApartmentElementRepository>();
                services.AddTransient<IPresentationApartmentPanelRepository, PresentationApartmentPanelRepository>();
            });
            return hostBuilder;
        }
    }
}
