using Autodesk.Revit.UI;
using DockableDialogs.Domain;
using DockableDialogs.Domain.Models;
using DockableDialogs.ViewModel.ComponentsVM;
using System.Collections.ObjectModel;

namespace DockableDialogs.ViewModel
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
