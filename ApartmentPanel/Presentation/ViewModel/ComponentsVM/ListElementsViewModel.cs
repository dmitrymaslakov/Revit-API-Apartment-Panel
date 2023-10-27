﻿using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Presentation.Commands;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM
{
    internal class ListElementsViewModel : ViewModelBase, IListElementsViewModel
    {
        public ListElementsViewModel() { }

        public ListElementsViewModel(IElementService elementService) : base(elementService)
        {
            AddToApartmentCommand = new RelayCommand(selectedTreeElement =>
            {
                try
                {
                    if (selectedTreeElement is IApartmentElement selectedElement)
                        AddElementToApartment(selectedElement);
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("exeption", ex.Message);
                }
            });
        }

        public Action<IApartmentElement> AddElementToApartment { get; set; }

        private ObservableCollection<IApartmentElement> _allElements;

        public ObservableCollection<IApartmentElement> AllElements
        {
            get => _allElements;
            set => Set(ref _allElements, value);
        }

        public ICommand AddToApartmentCommand { get; set; }
    }
}
