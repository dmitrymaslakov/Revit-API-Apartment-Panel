using ApartmentPanel.Utility.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Core.Models.Interfaces
{
    public interface IApartmentElement : IDeepClone<IApartmentElement>
    {
        event PropertyChangedEventHandler AnnotationChanged;
        void AnnotationChanged_Handler(object sender, PropertyChangedEventArgs args);
        bool IsSubscriber { get; set; }
        BitmapImage Annotation { get; set; }
        string Category { get; set; }
        string Family { get; set; }
        string Name { get; set; }
        Height MountingHeight { get; set; }
        ObservableCollection<Parameter> Parameters { get; set; }
    }
}