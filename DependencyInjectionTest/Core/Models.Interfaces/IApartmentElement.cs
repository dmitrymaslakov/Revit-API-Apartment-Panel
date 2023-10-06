using System.ComponentModel;
using System.Windows.Media;

namespace DependencyInjectionTest.Core.Models.Interfaces
{
    public interface IApartmentElement : INotifyPropertyChanged
    {
        ImageSource Annotation { get; set; }
        string Category { get; set; }
        string Name { get; set; }

        IApartmentElement Clone();
    }
}