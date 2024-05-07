using System.Windows.Media.Imaging;
using System.Windows.Media;
using ApartmentPanel.Utility.AnnotationUtility.Interfaces;

namespace ApartmentPanel.Utility.AnnotationUtility
{
    public class AnnotationService : IAnnotationService
    {
        private readonly IAnnotationCommunicatorFactory _annotationCommunicatorFactory;

        public AnnotationService(IAnnotationCommunicatorFactory annotationCommunicatorFactory)
            => _annotationCommunicatorFactory = annotationCommunicatorFactory;

        public BitmapImage Get()
        {
            using (IAnnotationReader reader = _annotationCommunicatorFactory.CreateAnnotationReader())
            {
                return reader.Get();
            }
        }
        public BitmapImage Save(BitmapImage annotation)
        {
            using (IAnnotationWriter writer = _annotationCommunicatorFactory.CreateAnnotationWriter())
            {
                return writer.Save(annotation);
            }
        }
        public bool IsAnnotationExists() => _annotationCommunicatorFactory.IsAnnotationExists();
    }
}
