using System;
using System.Windows.Media;

namespace WpfPanel.Domain.Services.AnnotationService.AnnotationReaders
{
    public interface IAnnotationReader : IDisposable
    {
        ImageSource Get();
    }
}