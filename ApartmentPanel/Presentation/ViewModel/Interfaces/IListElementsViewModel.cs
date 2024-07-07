using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.Interfaces
{
    public interface IListElementsViewModel
    {
        Action<ElectricalElement> AddElementToApartment { get; set; }
        ObservableCollection<ElectricalElement> AllElements { get; set; }
        ICommand AddToApartmentCommand { get; set; }
    }
}