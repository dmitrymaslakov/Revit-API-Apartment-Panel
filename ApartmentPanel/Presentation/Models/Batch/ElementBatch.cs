using ApartmentPanel.Presentation.Enums;
using ApartmentPanel.Presentation.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public ImageSource Annotation
        {
            get => _annotation;
            set => Set(ref _annotation, value);
        }

        private Direction _direction;
        public Direction Direction
        {
            get => _direction;
            set => Set(ref _direction, value);
        }


        public ElementBatch Clone()
        {
            return new ElementBatch
            {
                Name = Name,
                Annotation = Annotation?.Clone(),
                Direction = Direction,
                BatchedRows = new ObservableCollection<BatchedRow>(BatchedRows.Select(row => row.Clone())),
            };
        }
    }
}
