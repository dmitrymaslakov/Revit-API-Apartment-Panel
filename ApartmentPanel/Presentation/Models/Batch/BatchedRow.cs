using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ApartmentPanel.Presentation.Models.Batch
{
    public class BatchedRow : ViewModelBase
    {
        public BatchedRow()
        {
            RowElements = new ObservableCollection<BatchedElement> ();
            HeightFromFloor = new Height();
            Margins = new List<Thickness> ();
        }
        public int Number { get; set; }
        public Height HeightFromFloor { get; set; }
        private ObservableCollection<BatchedElement> _rowElements;
        public ObservableCollection<BatchedElement> RowElements
        {
            get => _rowElements;
            set => Set(ref _rowElements, value);
        }
        public List<Thickness> Margins { get; set; }

        public BatchedRow Clone()
        {
            return new BatchedRow
            {
                Number = Number,
                HeightFromFloor = new Height { TypeOf = HeightFromFloor.TypeOf, Value = HeightFromFloor.Value },
                RowElements = new ObservableCollection<BatchedElement>(RowElements.Select(e => e.Clone()))
            };
        }
    }
}
