using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Utility.AnnotationUtility.Interfaces
{
    public interface IAnnotationService
    {
        ImageSource Get();
        bool IsAnnotationExists();
        BitmapSource Save(BitmapSource annotation);
    }
}