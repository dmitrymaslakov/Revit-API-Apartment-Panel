using ApartmentPanel.Infrastructure.Repositories;
using Autodesk.Revit.UI;
using ApartmentPanel.Core.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApartmentPanel.Infrastructure
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddInfrastructureServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<RequestHandler>();
                services.AddSingleton(provider =>
                    ExternalEvent.Create(provider.GetService<RequestHandler>()));
                services.AddTransient<IInfrastructureElementRepository, InfrastructureElementRepository>();
            });
            return hostBuilder;
        }
    }
}
