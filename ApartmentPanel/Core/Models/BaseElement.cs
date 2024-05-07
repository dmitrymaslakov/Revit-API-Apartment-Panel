using ApartmentPanel.Utility;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Core.Models
{
    public abstract class BaseElement : NotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler AnnotationChanged;
        public bool IsSubscriber { get; set; }
        protected void OnAnnotationChanged() =>
            AnnotationChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Annotation)));

        public string Name { get; set; }
        public string Category { get; set; }        
        public string Family { get; set; }
        private BitmapImage _annotation;
        [JsonIgnore]
        public BitmapImage Annotation
        {
            get => _annotation;
            set
            {
                bool isSet = Set(ref _annotation, value);
                if (isSet) OnAnnotationChanged();
            }
        }
        private ObservableCollection<Parameter> _parameters;
        public ObservableCollection<Parameter> Parameters
        {
            get => _parameters;
            set => Set(ref _parameters, value);
        }
        public void AnnotationChanged_Handler(object sender, PropertyChangedEventArgs args)
        {
            BaseElement baseElement = (BaseElement)sender;
            Annotation = baseElement.Annotation;
        }
    }
}
