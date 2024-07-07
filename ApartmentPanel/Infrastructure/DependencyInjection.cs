using ApartmentPanel.Infrastructure.Repositories;
using ApartmentPanel.Infrastructure.Handler;
using Autodesk.Revit.UI;
using ApartmentPanel.Core.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApartmentPanel.RevitInfrastructure.Handler;

namespace ApartmentPanel.Infrastructure
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddInfrastructureServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<ExternalEventHandler>();
                services.AddSingleton(provider =>
                    ExternalEvent.Create(provider.GetService<ExternalEventHandler>()));
                services.AddTransient<IElementRepository, InfrastructureElementRepository>();
            });
            return hostBuilder;
        }
    }
}
