using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Utility.AnnotationUtility.Interfaces
{
    public interface IAnnotationService
    {
        bool IsAnnotationExists();
        BitmapImage Get();
        BitmapImage Save(BitmapImage annotation);
    }
}