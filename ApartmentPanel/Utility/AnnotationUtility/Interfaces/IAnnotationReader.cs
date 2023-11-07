using System;
using System.Windows.Media;

namespace ApartmentPanel.Utility.AnnotationUtility.Interfaces
{
    public interface IAnnotationReader : IDisposable
    {
        ImageSource Get();
    }
}
