using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.Interfaces
{
    public interface IListElementsViewModel
    {
        ObservableCollection<IApartmentElement> AllElements { get; set; }
        ICommand AddToApartmentCommand { get; set; }
        Action<IApartmentElement> AddElementToApartment { get; set; }
    }
}