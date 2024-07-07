using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using ApartmentPanel.UseCases.Configs.Dto;
using ApartmentPanel.UseCases.Configs.Queries.GetConfig;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApartmentPanel.UseCases.Configs.Commands.CreateConfig
{
    public class GetConfigRequestHandler : IRequestHandler<GetConfigRequest, GetConfigDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetConfigRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetConfigDto> Handle(GetConfigRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            string configName = request.ConfigName;
            if (!string.Equals(_unitOfWork.UsedFileDb, configName))
                _unitOfWork.UseFileDb(configName);
            IDbContext dbContext = _unitOfWork.GetDbContext();
            var dto = _mapper.Map<GetConfigDto>(dbContext);
            return dto;
        }
    }
}
