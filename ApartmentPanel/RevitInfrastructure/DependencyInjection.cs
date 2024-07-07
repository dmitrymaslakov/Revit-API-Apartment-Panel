using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ApartmentPanel.Infrastructure.Interfaces.Services;
using ApartmentPanel.RevitInfrastructure.Services;
using Revit.Async;

namespace ApartmentPanel.RevitInfrastructure
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddRevitInfrastructureServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddScoped<RevitTask>();
                services.AddTransient<ICadServices, RevitService> ();
            });
            return hostBuilder;
        }
    }
}
