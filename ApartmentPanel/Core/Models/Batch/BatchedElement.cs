using ApartmentPanel.Utility.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ApartmentPanel.Core.Models.Batch
{
    public class BatchedElement : BaseElement, IDeepClone<BatchedElement>
    {
        public BatchedElement() =>
            Margin = new Thickness(0, 0, 0, 0);
        //Margin = new BatchedMargin(0, 0, 0, 0);

        public string Circuit { get; set; }

        /*private BatchedMargin _margin;
        public BatchedMargin Margin*/
        private Thickness _margin;
        public Thickness Margin
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
                Family = Family,
                Circuit = Circuit,
                Annotation = Annotation?.Clone(),
                Margin = Margin,
                Parameters = new ObservableCollection<Parameter>(Parameters?.Select(p => p.Clone())?.ToList())
            };
        }
    }
}
