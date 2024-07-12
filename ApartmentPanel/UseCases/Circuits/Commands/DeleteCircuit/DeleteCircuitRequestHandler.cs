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

namespace ApartmentPanel.UseCases.Circuits.Commands.DeleteCircuit
{
    public class DeleteCircuitRequestHandler
        : IRequestHandler<DeleteCircuitRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCircuitRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteCircuitRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            bool isDeleted = _unitOfWork.CircuitRepository.Delete(request.CircuitNumber);
            _unitOfWork.SaveChanges();
            return isDeleted;
        }
    }
}
