using ApartmentPanel.Core.Services.AnnotationService.AnnotationReaders;
using ApartmentPanel.Core.Services.AnnotationService.AnnotationWriters;

namespace ApartmentPanel.Core.Services.AnnotationService
{
    public interface IAnnotationCommunicatorFactory
    {
        IAnnotationReader CreateAnnotationReader();
        IAnnotationWriter CreateAnnotationWriter();
        bool IsFileExists();
    }
}
