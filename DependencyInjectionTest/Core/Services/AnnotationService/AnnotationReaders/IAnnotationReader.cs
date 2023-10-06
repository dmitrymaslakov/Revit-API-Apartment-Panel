using System;
using System.Windows.Media;

namespace DependencyInjectionTest.Core.Services.AnnotationService.AnnotationReaders
{
    public interface IAnnotationReader : IDisposable
    {
        ImageSource Get();
    }
}
