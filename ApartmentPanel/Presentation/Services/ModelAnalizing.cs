using ApartmentPanel.Core.Infrastructure.Interfaces;

namespace ApartmentPanel.Presentation.Services
{
    public class ModelAnalizing
    {
        private readonly IInfrastructureElementRepository _repository;

        public ModelAnalizing(IInfrastructureElementRepository repository) => 
            _repository = repository;

        public void AnalizeElement() => _repository.Analize();
    }
}
