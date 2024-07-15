using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using MediatR;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.Circuits.Commands.CreateCircuit
{
    public class CreateCircuitRequestHandler
        : IRequestHandler<CreateCircuitRequest, Circuit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCircuitRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Circuit> Handle(CreateCircuitRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            var newCircuit = new Circuit
                {
                    Number = request.CircuitNumber,
                    Elements = new ObservableCollection<IApartmentElement>()
                };

            _unitOfWork.CircuitRepository.Add(newCircuit);
            return newCircuit;
        }
    }
}
