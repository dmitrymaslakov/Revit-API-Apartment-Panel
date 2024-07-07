using ApartmentPanel.Presentation.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApartmentPanel.Presentation.View;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Presentation.Services;
using MediatR;
using System.Reflection;
using ApartmentPanel.UseCases.ApartmentElements.Queries.GetApartmentElements;

namespace ApartmentPanel.Presentation
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddPresentationServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services => 
            {
                services.AddScoped<IListElementsViewModel, ListElementsViewModel>();
                services.AddScoped<IConfigPanelViewModel, ConfigPanelViewModel>();
                services.AddScoped<IMainViewModel, MainViewModel>();
                services.AddScoped(provider => new MainView(provider.GetService<IMainViewModel>()));
                services.AddScoped<ModelAnalizing>();
            });
            return hostBuilder;
        }
    }
}
