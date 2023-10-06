using DependencyInjectionTest.Core.Services.AnnotationService.AnnotationReaders;
using DependencyInjectionTest.Core.Services.AnnotationService.AnnotationWriters;

namespace DependencyInjectionTest.Core.Services.AnnotationService
{
    public interface IAnnotationCommunicatorFactory
    {
        IAnnotationReader CreateAnnotationReader();
        IAnnotationWriter CreateAnnotationWriter();
        bool IsFileExists();
    }
}
