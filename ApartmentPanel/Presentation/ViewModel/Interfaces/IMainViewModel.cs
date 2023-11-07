using ApartmentPanel.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.Interfaces
{
    public interface IMainViewModel
    {
        ObservableCollection<Circuit> Circuits { get; set; }
        IConfigPanelViewModel ConfigPanelVM { get; set; }
        string CurrentSuffix { get; set; }
        double HeightTypeOfUK { get; set; }
        string Status { get; set; }
        ICommand ConfigureCommand { get; set; }
        ICommand InsertElementCommand { get; set; }
        ICommand SetCurrentSuffixCommand { get; set; }
    }
}