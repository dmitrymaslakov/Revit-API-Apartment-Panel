using Autodesk.Revit.UI;
using System.Collections.ObjectModel;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Infrastructure;
using ApartmentPanel.Presentation.Models;

namespace ApartmentPanel.Presentation.ViewModel.Interfaces
{
    public interface IViewPropsForCommandsCreater
    {
        ObservableCollection<Circuit> Circuits { get; set; }
        string CurrentSuffix { get; set; }
        double Height { get; set; }
        string Status { get; set; }
        ConfigPanelViewModel ConfigPanelVM { get; }
        ExternalEvent ExEvent { get; }
        RequestHandler Handler { get; }
    }
}
