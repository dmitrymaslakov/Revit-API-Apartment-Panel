using ApartmentPanel.Presentation.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApartmentPanel.Presentation.View;

namespace ApartmentPanel.Presentation
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddViewService(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton(provider => new MainView(provider.GetService<MainViewModel>()));
            });
            return hostBuilder;
        }
    }
}
