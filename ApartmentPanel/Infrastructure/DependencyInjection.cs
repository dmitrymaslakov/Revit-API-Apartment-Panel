using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Infrastructure.Repositories;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApartmentPanel.Infrastructure
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddRepositoriesService(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<IApartmentElementRepository, ApartmentElementRepository>();
            });
            return hostBuilder;
        }
        public static IHostBuilder AddExternalEventHandlerService(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<RequestHandler>();
                services.AddSingleton(provider =>
                    ExternalEvent.Create(provider.GetService<RequestHandler>()));
            });
            return hostBuilder;
        }
    }
}
