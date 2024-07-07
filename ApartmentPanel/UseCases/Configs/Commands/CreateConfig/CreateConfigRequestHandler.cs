using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.Configs.Commands.CreateConfig
{
    public class CreateConfigRequestHandler : IRequestHandler<CreateConfigRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateConfigRequestHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(CreateConfigRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            return _unitOfWork.CreateDatabase(request.ConfigName);
        }
    }
}
