using System.ComponentModel;
using System.Windows.Media;

namespace ApartmentPanel.Core.Models.Interfaces
{
    public interface IApartmentElement : INotifyPropertyChanged
    {
        ImageSource Annotation { get; set; }
        string Category { get; set; }
        string Name { get; set; }
        Height MountingHeight { get; set; }

        IApartmentElement Clone();
    }
}