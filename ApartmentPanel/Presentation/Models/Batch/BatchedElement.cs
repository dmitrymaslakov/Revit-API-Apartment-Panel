using ApartmentPanel.Presentation.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;

namespace ApartmentPanel.Presentation.Models.Batch
{
    public class BatchedElement : ViewModelBase
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Circuit { get; set; }
        [JsonIgnore]
        public ImageSource Annotation { get; set; }
        public BatchedLocation Location { get; set; }

        private BatchedMargin _margin;
        public BatchedMargin Margin
        {
            get => _margin;
            set => Set(ref _margin, value);
        }
        /*private Thickness _margin;
        public Thickness Margin
        {
            get => _margin;
            set => Set(ref _margin, value);
        }*/

        private ObservableCollection<Parameter> _parameters;
        public ObservableCollection<Parameter> Parameters
        {
            get => _parameters;
            set => Set(ref _parameters, value);
        }

        public BatchedElement Clone()
        {
            return new BatchedElement
            {
                Name = Name,
                Category = Category,
                Circuit = Circuit,
                Annotation = Annotation?.Clone(),
                Location = Location,
                Margin = Margin,
                Parameters = new ObservableCollection<Parameter>(Parameters?.ToList())
            };
        }
    }
}
