using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Core.Services;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Infrastructure;
using ApartmentPanel.Infrastructure.Repositories;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApartmentPanel.Core
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddApartmentElementService(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<IApartmentElementService, ApartmentElementService>();
            });
            return hostBuilder;
        }
    }
}
