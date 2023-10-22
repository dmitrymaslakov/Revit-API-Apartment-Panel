using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Services;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Presentation.Commands;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM
{
    internal class ListElementsViewModel : ViewModelBase, IListElementsViewModel
    {
        public ListElementsViewModel() { }

        public ListElementsViewModel(IElementService elementService) : base(elementService)
        {
            AddElementToApartment = new RelayCommand(selectedElement => )
        }

        private ObservableCollection<IApartmentElement> _allElements;

        public ObservableCollection<IApartmentElement> AllElements
        {
            get => _allElements;
            set => Set(ref _allElements, value);
        }

        private IApartmentElement _selectedElement;

        public IApartmentElement SelectedElement
        {
            get => _selectedElement;
            set => Set(ref _selectedElement, value);
        }

        public ICommand AddElementToApartment { get; set; }

    }
}
