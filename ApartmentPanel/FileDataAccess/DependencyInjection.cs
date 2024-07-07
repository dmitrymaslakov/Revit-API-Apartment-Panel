using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using ApartmentPanel.FileDataAccess.Services;

namespace ApartmentPanel.FileDataAccess
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddFileDataAccessServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                //services.AddScoped<FileDbContext>();
                services.AddScoped<FileDbContextFactory>(); 
                services.AddScoped<IUnitOfWork, ApartmentPanelUOW>();
            });
            return hostBuilder;
        }
    }
}
