using ApartmentPanel.Core.Infrastructure.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;

namespace ApartmentPanel.Core.Services
{
    public class ApartmentElementService : IApartmentElementService
    {
        private readonly IApartmentElementRepository _apartmentElementRepository;
        public ApartmentElementService(IApartmentElementRepository apartmentElementRepository)
        {
            _apartmentElementRepository = apartmentElementRepository;
        }

        public void Insert()
        {
            _apartmentElementRepository.InsertElement();
        }
    }
}
