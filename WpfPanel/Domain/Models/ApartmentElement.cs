using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace WpfPanel.Domain.Models
{
    public class ApartmentElement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Category { get; set; }
        private ImageSource _annotation;
        public ImageSource Annotation { get; set; }
        /*public ImageSource Annotation
        { 
            get => _annotation; 
            set
            {
                _annotation = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/
    }
}
