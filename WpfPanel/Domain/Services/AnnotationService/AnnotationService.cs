using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfPanel.Domain.Services.AnnotationService.AnnotationReaders;
using WpfPanel.Domain.Services.AnnotationService.AnnotationWriters;

namespace WpfPanel.Domain.Services.AnnotationService
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
