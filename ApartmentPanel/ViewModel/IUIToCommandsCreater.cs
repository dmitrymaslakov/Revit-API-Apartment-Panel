using Autodesk.Revit.UI;
using ApartmentPanel.Domain;
using ApartmentPanel.Domain.Models;
using ApartmentPanel.ViewModel.ComponentsVM;
using System.Collections.ObjectModel;

namespace ApartmentPanel.ViewModel
{
    public interface IUIToCommandsCreater
    {
        ObservableCollection<Circuit> Circuits { get; set; }
        string CurrentSuffix { get; set; }
        double Height { get; set; }
        string Status { get; set; }
        EditPanelVM EditPanelVM { get; }
        ExternalEvent ExEvent { get; }
        RequestHandler Handler { get; }
    }
}
