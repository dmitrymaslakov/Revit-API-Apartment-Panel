using System;
using System.Windows.Media;

namespace ApartmentPanel.Core.Services.AnnotationService.AnnotationReaders
{
    public interface IAnnotationReader : IDisposable
    {
        ImageSource Get();
    }
}
