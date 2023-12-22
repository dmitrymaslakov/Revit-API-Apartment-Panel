using ApartmentPanel.Presentation.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace ApartmentPanel.Presentation.Models.Batch
{
    public class ElementBatch : ViewModelBase
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

        public ElementBatch Clone()
        {
            return new ElementBatch
            {
                Name = Name,
                Annotation = Annotation?.Clone(),
                BatchedRows = new ObservableCollection<BatchedRow>(BatchedRows.Select(row => row.Clone())),
            };
        }
    }
}
