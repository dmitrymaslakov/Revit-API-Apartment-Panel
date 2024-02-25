using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Core.Models.Interfaces
{
    public interface IApartmentElement
    {
        //ImageSource Annotation { get; set; }
        BitmapImage Annotation { get; set; }
        string Category { get; set; }
        string Name { get; set; }
        Height MountingHeight { get; set; }
        ObservableCollection<Parameter> Parameters { get; set; }

        IApartmentElement Clone();
    }
}