using Autodesk.Revit.UI;
using System.Collections.ObjectModel;
using DependencyInjectionTest.Presentation.ViewModel.ComponentsVM;
using DependencyInjectionTest.Infrastructure;
using DependencyInjectionTest.Core.Models;

namespace DependencyInjectionTest.Presentation.ViewModel.Interfaces
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
