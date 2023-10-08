using ApartmentPanel.Core.Services;
using ApartmentPanel.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Models;

namespace ApartmentPanel.Core
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddDomainServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<IApartmentElement, ApartmentElement>();
                services.AddTransient<IElementService, ElementService>();
                services.AddTransient<IPanelService, PanelService>();
            });
            return hostBuilder;
        }
    }
}
