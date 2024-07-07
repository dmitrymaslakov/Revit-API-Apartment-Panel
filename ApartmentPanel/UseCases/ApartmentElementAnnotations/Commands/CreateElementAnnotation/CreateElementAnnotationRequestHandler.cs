using ApartmentPanel.UseCases.ApartmentElementAnnotations.Commands.CreateElementAnnotation;
using ApartmentPanel.Utility;
using ApartmentPanel.Utility.AnnotationUtility;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.UseCases.ApartmentElements.Commands.AddElementToApartment
{
    public class CreateElementAnnotationRequestHandler
        : IRequestHandler<CreateElementAnnotationRequest, BitmapImage>
    {
        private readonly string _annotationFolder = FileUtility.GetApplicationAnnotationsFolder();
        private readonly string _deshSeparator = "-";

        public async Task<BitmapImage> Handle(CreateElementAnnotationRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            string annotationName = new PathBuilder()
                .AddFolders(_annotationFolder, request.Config)
                .AddPartsOfName(_deshSeparator, request.Family, request.Name)
                .AddPngExtension()
                .Build();

            var annotationService = new AnnotationService(
            new FileAnnotationCommunicatorFactory(annotationName));
            return annotationService.Save(request.Annotation);
        }
    }
}
