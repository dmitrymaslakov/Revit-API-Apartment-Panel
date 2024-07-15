using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using MediatR;
using System.Linq;
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
            var forDelete = _unitOfWork.CircuitRepository
                .FindBy(c => string.Equals(c.Number, request.CircuitNumber))
                .FirstOrDefault();
            return _unitOfWork.CircuitRepository.Delete(forDelete);
        }
    }
}
