using DependencyInjectionTest.Infrastructure.Repositories;
using Autodesk.Revit.UI;
using DependencyInjectionTest.Core.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DependencyInjectionTest.Core.Presentation.Interfaces;

namespace DependencyInjectionTest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddInfrastructureServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<IPresentationApartmentPanelRepository, InfrastructureApartmentElementRepository>();
                services.AddTransient<IInfrastructureApartmentPanelRepository, ApartmentPanelRepository>();
                services.AddSingleton<RequestHandler>();
                services.AddSingleton(provider =>
                    ExternalEvent.Create(provider.GetService<RequestHandler>()));
            });
            return hostBuilder;
        }
    }
}
