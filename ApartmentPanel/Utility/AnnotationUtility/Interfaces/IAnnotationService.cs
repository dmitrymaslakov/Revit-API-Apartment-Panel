using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Utility.AnnotationUtility.Interfaces
{
    public interface IAnnotationService
    {
        bool IsAnnotationExists();
        //ImageSource Get();
        BitmapImage Get();
        //BitmapSource Save(BitmapSource annotation);
        BitmapImage Save(BitmapImage annotation);
    }
}