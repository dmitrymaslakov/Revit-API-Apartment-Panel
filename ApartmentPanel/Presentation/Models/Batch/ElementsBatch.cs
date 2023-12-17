using ApartmentPanel.Presentation.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ApartmentPanel.Presentation.Models.Batch
{
    public class ElementsBatch : ViewModelBase
    {
        public ObservableCollection<BatchedRow> BatchedRows { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private ImageSource _annotation;
        public ImageSource Annotation
        {
            get => _annotation;
            set => Set(ref _annotation, value);
        }

    }
}
