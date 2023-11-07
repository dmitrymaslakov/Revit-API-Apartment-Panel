using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApartmentPanel.Utility.AnnotationUtility.Interfaces;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using ApartmentPanel.Utility.AnnotationUtility;

namespace ApartmentPanel.Utility
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddAnnotationService(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<IAnnotationReader, FileAnnotationReader>();
                services.AddTransient<IAnnotationWriter, FileAnnotationWriter>();
                services.AddTransient<IAnnotationCommunicatorFactory, FileAnnotationCommunicatorFactory>();
                services.AddTransient<IAnnotationService, AnnotationService>();
            });
            return hostBuilder;
        }
    }
}
