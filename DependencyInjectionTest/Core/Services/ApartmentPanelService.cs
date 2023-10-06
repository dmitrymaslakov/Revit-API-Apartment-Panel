using DependencyInjectionTest.Core.Infrastructure.Interfaces;
using DependencyInjectionTest.Core.Presentation.Interfaces;
using DependencyInjectionTest.Core.Services.Interfaces;

namespace DependencyInjectionTest.Core.Services
{
    public class ApartmentPanelService : IApartmentPanelService
    {
        private readonly IPresentationApartmentPanelRepository _apartmentPanelRepository;

        public ApartmentPanelService(IPresentationApartmentPanelRepository apartmentPanelRepository) =>
            _apartmentPanelRepository = apartmentPanelRepository;

        public void Configure()
        {
            _apartmentPanelRepository.ConfigurePanel();
        }
    }
}
