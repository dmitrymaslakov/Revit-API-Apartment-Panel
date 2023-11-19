using ApartmentPanel.Core.Infrastructure.Interfaces;

namespace ApartmentPanel.Presentation.Services
{
    public class ModelAnalizing
    {
        private readonly IElementRepository _repository;

        public ModelAnalizing(IElementRepository repository) => 
            _repository = repository;

        public void AnalizeElement() => _repository.Analize();
    }
}
