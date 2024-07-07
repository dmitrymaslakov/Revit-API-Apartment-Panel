using ApartmentPanel.Presentation.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApartmentPanel.Presentation.View;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Presentation.Services;
using MediatR;
using System.Reflection;
using ApartmentPanel.UseCases.ApartmentElements.Queries.GetApartmentElements;
using System;
using ApartmentPanel.UseCases.ElectricalElements.Queries.GetElectricalElements;

namespace ApartmentPanel.UseCases
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddUseCaseServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddMediatR(typeof(GetElectricalElementsRequestHandler).Assembly);
            });
            return hostBuilder;
        }
    }
}
