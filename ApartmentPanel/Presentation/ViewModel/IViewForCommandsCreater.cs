using Autodesk.Revit.UI;
using ApartmentPanel.Core.Models;
using System.Collections.ObjectModel;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Infrastructure;

namespace ApartmentPanel.Presentation.ViewModel
{
    public interface IViewForCommandsCreater
    {
        ObservableCollection<Circuit> Circuits { get; set; }
        string CurrentSuffix { get; set; }
        double Height { get; set; }
        string Status { get; set; }
        ConfigPanelVM EditPanelVM { get; }
        ExternalEvent ExEvent { get; }
        RequestHandler Handler { get; }
    }
}
