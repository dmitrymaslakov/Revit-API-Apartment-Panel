using ApartmentPanel.Core.Models.Interfaces;
using System.Collections.ObjectModel;

namespace ApartmentPanel.Presentation.ViewModel.Interfaces
{
    public interface IListElementsViewModel
    {
        ObservableCollection<IApartmentElement> AllElements { get; set; }
    }
}