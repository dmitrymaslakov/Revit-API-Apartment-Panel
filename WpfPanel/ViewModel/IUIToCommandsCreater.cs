using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.ViewModel.ComponentsVM;

namespace WpfPanel.ViewModel
{
    public interface IUIToCommandsCreater
    {
        ObservableCollection<Circuit> Circuits { get; set; }
        string CurrentSuffix { get; set; }
        double Height { get; set; }
        string LatestConfigPath { get; }
        string Status { get; set; }
        EditPanelVM EditPanelVM { get; }
        ExternalEvent ExEvent { get; }
        RequestHandler Handler { get; }
    }
}