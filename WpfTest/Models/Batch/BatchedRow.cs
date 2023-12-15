using System.Collections.ObjectModel;
using WpfTest.ViewModels;

namespace WpfTest.Models.Batch
{
    public class BatchedRow : ViewModelBase
    {
        public int Number { get; set; }
        public Height HeightFromFloor { get; set; }
        private ObservableCollection<BatchedElement> _rowElements;
        public ObservableCollection<BatchedElement> RowElements
        {
            get => _rowElements;
            set => Set(ref _rowElements, value);
        }

        //public List<Thickness> Margins { get; set; }
    }
}
