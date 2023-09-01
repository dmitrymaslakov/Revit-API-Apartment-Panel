using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.Utilities;

namespace WpfPanel.ViewModel.ComponentsVM
{
    public interface IEditPanelToCommandsCreater
    {
        ExternalEvent ExEvent { get; }
        RequestHandler Handler { get; }
        BitmapSource AnnotationPreview { get; set; }
        ObservableCollection<ApartmentElement> ApartmentElements { get; set; }
        ObservableCollection<ApartmentElement> CircuitElements { get; set; }
        string NewCircuit { get; set; }
        Action<object, OkApplyCancel> OkApplyCancelActions { get; }
        ObservableDictionary<string, ObservableCollection<ApartmentElement>> PanelCircuits { get; set; }
        ObservableCollection<ApartmentElement> SelectedApartmentElements { get; set; }
        ObservableCollection<ApartmentElement> SelectedCircuitElements { get; set; }
        ObservableCollection<KeyValuePair<string, ObservableCollection<ApartmentElement>>> SelectedPanelCircuits { get; set; }
        EditPanelVM ApplyLatestConfiguration(EditPanelVM latestConfiguration);
    }
}