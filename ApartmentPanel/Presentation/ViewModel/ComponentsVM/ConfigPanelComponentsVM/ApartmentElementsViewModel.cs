using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Presentation.Commands;
using ApartmentPanel.Presentation.Commands.ConfigPanelCommands;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.ComponentsVM.ConfigPanelComponentsVM
{
    public class ApartmentElementsViewModel : ViewModelBase
    {
        private readonly ApartmentElementsCommandCreater _commandCreater;
        public ApartmentElementsViewModel() { }
        public ApartmentElementsViewModel(IConfigPanelViewModel configPanelVM, 
            IElementService elementService, IMediator mediator, IMapper mapper) 
        {
            _commandCreater 
                = new ApartmentElementsCommandCreater(configPanelVM, elementService, mediator, mapper);
            ApartmentElements = new ObservableCollection<IApartmentElement>();
            SelectedApartmentElements = new ObservableCollection<IApartmentElement>();
            ShowElementListCommand = _commandCreater.CreateShowElementListCommand();
            RemoveApartmentElementCommand = _commandCreater.CreateRemoveApartmentElementCommand();
            SelectApartmentElementCommand = _commandCreater.CreateSelectApartmentElementCommand();
        }

        private ObservableCollection<IApartmentElement> _apartmentElements;
        public ObservableCollection<IApartmentElement> ApartmentElements
        {
            get => _apartmentElements;
            set => Set(ref _apartmentElements, value);
        }

        private ObservableCollection<IApartmentElement> _selectedApartmentElements;
        [JsonIgnore]
        public ObservableCollection<IApartmentElement> SelectedApartmentElements
        {
            get => _selectedApartmentElements;
            set => Set(ref _selectedApartmentElements, value);
        }

        [JsonIgnore]
        public ICommand ShowElementListCommand { get; }
        [JsonIgnore]
        public ICommand RemoveApartmentElementCommand { get; }
        [JsonIgnore]
        public ICommand SelectApartmentElementCommand { get; }
    }
}
