using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using ApartmentPanel.UseCases.Configs.Dto;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.DataChanges.Commands.CancelCanges
{
    public class CancelCangesRequestHandler
        : IRequestHandler<CancelCangesRequest, GetConfigDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CancelCangesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetConfigDto> Handle(CancelCangesRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            IDbContext dbContext = _unitOfWork.GetDbContext();
            var dto = _mapper.Map<GetConfigDto>(dbContext);
            return dto;
        }
    }
}
