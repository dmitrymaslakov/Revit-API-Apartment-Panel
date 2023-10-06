using System.Windows.Media.Imaging;
using System.Windows.Media;
using DependencyInjectionTest.Core.Services.AnnotationService.AnnotationReaders;
using DependencyInjectionTest.Core.Services.AnnotationService.AnnotationWriters;

namespace DependencyInjectionTest.Core.Services.AnnotationService
{
    public class AnnotationService
    {
        private readonly IAnnotationCommunicatorFactory _annotationCommunicatorFactory;

        public AnnotationService(IAnnotationCommunicatorFactory annotationCommunicatorFactory)
            => _annotationCommunicatorFactory = annotationCommunicatorFactory;

        public ImageSource Get()
        {
            using (IAnnotationReader reader = _annotationCommunicatorFactory.CreateAnnotationReader())
            {
                return reader.Get();
            }
        }
        public BitmapSource Save(BitmapSource annotation)
        {
            using (IAnnotationWriter writer = _annotationCommunicatorFactory.CreateAnnotationWriter())
            {
                return writer.Save(annotation);
            }
        }
        public bool IsAnnotationExists() => _annotationCommunicatorFactory.IsFileExists();
    }
}
