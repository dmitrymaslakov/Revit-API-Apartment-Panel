using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.DataChanges.Commands.SaveChanges
{
    public class SaveChangesRequestHandler
        : IRequestHandler<SaveChangesRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveChangesRequestHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(SaveChangesRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);            
            return _unitOfWork.SaveChanges();
        }
    }
}
