using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.Configs.Commands.UpdateConfig
{
    public class UpdateConfigRequestHandler : IRequestHandler<UpdateConfigRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateConfigRequestHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(UpdateConfigRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            var dto = request.ConfigDto;
            _unitOfWork.ApartmentElementRepository.Clear();
            _unitOfWork.ApartmentElementRepository.AddRange(dto.ApartmentElements);
            _unitOfWork.ElementBatchRepository.Clear();
            _unitOfWork.ElementBatchRepository.AddRange(dto.ElementBatches);
            _unitOfWork.CircuitRepository.Clear();
            _unitOfWork.CircuitRepository.AddRange(dto.Circuits);
            _unitOfWork.HeightRepository.Clear();
            _unitOfWork.HeightRepository.AddRange(dto.Heights);
            _unitOfWork.ResponsibleForHeightRepository.Clear();
            _unitOfWork.ResponsibleForHeightRepository.AddRange(dto.ResponsibleForHeights);
            _unitOfWork.ResponsibleForCircuitRepository.Clear();
            _unitOfWork.ResponsibleForCircuitRepository.AddRange(dto.ResponsibleForCircuits);
            _unitOfWork.SaveChanges();
            return Unit.Value;
        }
    }
}
