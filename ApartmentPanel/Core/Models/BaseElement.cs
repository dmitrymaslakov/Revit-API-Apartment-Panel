using ApartmentPanel.Utility;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Core.Models
{
    public abstract class BaseElement : NotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Category { get; set; }
        
        //private ImageSource _annotation;
        private BitmapImage _annotation;
        [JsonIgnore]
        //public ImageSource Annotation
        public BitmapImage Annotation
        {
            get => _annotation;
            set => Set(ref _annotation, value);
        }
        private ObservableCollection<Parameter> _parameters;
        public ObservableCollection<Parameter> Parameters
        {
            get => _parameters;
            set => Set(ref _parameters, value);
        }

    }
}
