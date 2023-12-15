using ApartmentPanel.Presentation.ViewModel;
using System.Collections.ObjectModel;

namespace ApartmentPanel.Presentation.Models.Batch
{
    public class ElementsBatch : ViewModelBase
    {
        public ObservableCollection<BatchedRow> BatchedRows { get; set; }
    }
}
