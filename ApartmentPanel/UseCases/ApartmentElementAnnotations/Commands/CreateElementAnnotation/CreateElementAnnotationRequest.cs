using MediatR;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.UseCases.ApartmentElementAnnotations.Commands.CreateElementAnnotation
{
    public class CreateElementAnnotationRequest : IRequest<BitmapImage>
    {
        public string Name { get; set; }
        public string Config { get; set; }
        public string Family { get; set; }
        public BitmapImage Annotation { get; set; }
    }
}
