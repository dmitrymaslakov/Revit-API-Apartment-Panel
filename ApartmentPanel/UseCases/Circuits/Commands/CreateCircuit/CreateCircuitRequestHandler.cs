using ApartmentPanel.Core.Models;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using ApartmentPanel.UseCases.ApartmentElements.Commands.Circuits;
using ApartmentPanel.Utility;
using ApartmentPanel.Utility.AnnotationUtility;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using AutoMapper;
using MediatR;
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
            _unitOfWork.SaveChanges();
            return apartmentElement;
        }
    }
}
