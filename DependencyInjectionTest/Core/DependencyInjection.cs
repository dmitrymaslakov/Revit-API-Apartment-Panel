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
                services.AddTransient<IElementService, ElementService>();
                services.AddTransient<IPanelService, PanelService>();
                services.AddTransient<IApartmentElement, ApartmentElement>();
            });
            return hostBuilder;
        }
    }
}
