using ApartmentPanel.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows.Media;
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
        //private ImageSource _annotation;
        private BitmapImage _annotation;
        [JsonIgnore]
        //public ImageSource Annotation
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
