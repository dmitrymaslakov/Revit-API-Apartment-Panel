using System.Collections.ObjectModel;
using WpfTest.ViewModels;

namespace WpfTest.Models.Batch
{
    public class ElementsBatch : ViewModelBase
    {
        public ObservableCollection<BatchedRow> BatchedRows { get; set; }
        /*private ObservableCollection<BatchedRow> _batchedRows;
        public ObservableCollection<BatchedRow> BatchedRows 
        { 
            get => _batchedRows; 
            set => Set(ref _batchedRows, value);
        }*/
    }
}
