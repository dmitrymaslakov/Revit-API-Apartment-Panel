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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _annotationFolder = FileUtility.GetApplicationAnnotationsFolder();
        private readonly string _deshSeparator = "-";

        public CreateCircuitRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Circuit> Handle(CreateCircuitRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            var dto = request.ApartmentElementCreateDto;
            var apartmentElement = _mapper.Map<ApartmentElement>(dto);
            string annotationName = new PathBuilder()
                .AddFolders(_annotationFolder, dto.Config)
                .AddPartsOfName(_deshSeparator, dto.Family, dto.Name)
                .AddPngExtension()
                .Build();

            var annotationService = new AnnotationService(
                new FileAnnotationCommunicatorFactory(annotationName));
            apartmentElement.Annotation = annotationService.IsAnnotationExists()
                ? annotationService.Get() : null;
            _unitOfWork.ApartmentElementRepository.Add(apartmentElement);
            _unitOfWork.SaveChanges();
            return apartmentElement;
        }
    }
}
