using ApartmentPanel.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApartmentPanel.Presentation.Models.Batch
{
    public class BatchedElement : BaseElement
    {
        public BatchedElement() => Margin = new BatchedMargin(0, 0, 0, 0);

        public string Circuit { get; set; }
        public BatchedLocation Location { get; set; }

        private BatchedMargin _margin;
        public BatchedMargin Margin
        {
            get => _margin;
            set => Set(ref _margin, value);
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
