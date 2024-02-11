using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Text.Json.Serialization;
using ApartmentPanel.Core.Models.Interfaces;

namespace ApartmentPanel.Core.Models
{
    public class ApartmentElement : IApartmentElement
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Category { get; set; }
        private ImageSource _annotation;
        [JsonIgnore]
        public ImageSource Annotation
        {
            get => _annotation;
            set => Set(ref _annotation, value);
        }
        public Height MountingHeight { get; set; }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public IApartmentElement Clone()
        {
            return new ApartmentElement
            {
                Name = Name,
                Category = Category,
                Annotation = Annotation?.Clone(),
                MountingHeight = MountingHeight.Clone()
            };
        }
    }
}
