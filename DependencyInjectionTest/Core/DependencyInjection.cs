using DependencyInjectionTest.Core.Services;
using DependencyInjectionTest.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DependencyInjectionTest.Core.Models.Interfaces;
using DependencyInjectionTest.Core.Models;

namespace DependencyInjectionTest.Core
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddDomainServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<IApartmentElementService, ApartmentElementService>();
                services.AddTransient<IApartmentPanelService, ApartmentPanelService>();
                services.AddTransient<IApartmentElement, ApartmentElement>();
            });
            return hostBuilder;
        }
    }
}
