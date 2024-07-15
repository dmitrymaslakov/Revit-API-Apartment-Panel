using ApartmentPanel.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApartmentPanel.Presentation.Services.ValidationServices
{
    internal class CircuitNumberValidation : IDataValidationStrategy
    {
        private readonly string _circuitNumber;
        private readonly ObservableCollection<Circuit> _panelCircuits;

        public CircuitNumberValidation(string circuitNumber, 
            ObservableCollection<Circuit> panelCircuits)
        {
            _circuitNumber = circuitNumber;
            _panelCircuits = panelCircuits;
        }
        public string Validate()
        {
            if (!string.IsNullOrEmpty(_circuitNumber)
                        && _panelCircuits.ToList().Exists(c => c.Number == _circuitNumber))
            {
                return $"The circuit '{_circuitNumber}' is already existed";
            }
            return null;
        }
    }
}
