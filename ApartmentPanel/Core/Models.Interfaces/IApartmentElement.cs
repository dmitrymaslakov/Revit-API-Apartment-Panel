using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace ApartmentPanel.Core.Models.Interfaces
{
    public interface IApartmentElement
    {
        ImageSource Annotation { get; set; }
        string Category { get; set; }
        string Name { get; set; }
        Height MountingHeight { get; set; }
        ObservableCollection<Parameter> Parameters { get; set; }

        IApartmentElement Clone();
    }
}