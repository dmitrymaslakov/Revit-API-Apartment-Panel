using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Services.Commands;

namespace WpfPanel.ViewModel.ComponentsVM
{
    public class EditPanelVM : ViewModelBase
    {
        private const string TRISSA_SWITCH = "Trissa Switch";
        private const string USB = "USB";
        private const string BLOCK1 = "BLOCK1";
        private const string SINGLE_SOCKET = "Single Socket";
        private const string THROUGH_SWITCH = "Through Switch";

        private readonly RequestHandler _handler;

        //public EditPanelVM(RequestHandler handler)
        public EditPanelVM()
        {
            //_handler = handler;

            ApartmentElements = new ObservableCollection<string>
            {
                TRISSA_SWITCH, USB, BLOCK1, SINGLE_SOCKET, THROUGH_SWITCH
            };
            PanelCircuits = new ObservableCollection<string>
            {
                "1", "2", "3"
            };
            CircuitElements = new ObservableCollection<string>
            {
                TRISSA_SWITCH, USB, BLOCK1, SINGLE_SOCKET, THROUGH_SWITCH
            };
            AddApartmentElement = new AddApartmentElementCommand(o => MessageBox.Show("Add element"));
            EditApartmentElement = new EditApartmentElementCommand(o => MessageBox.Show("Edit element"));
            RemoveApartmentElement = new RemoveApartmentElementCommand(o => MessageBox.Show("Remove element"));
            AddPanelCircuit = new AddPanelCircuitCommand(o => MessageBox.Show("Add circuit"));
            EditPanelCircuit = new EditPanelCircuitCommand(o => MessageBox.Show("Edit circuit"));
            RemovePanelCircuit = new RemovePanelCircuitCommand(o => MessageBox.Show("Remove circuit"));
        }

        private ObservableCollection<string> _apartmentElements;

        public ObservableCollection<string> ApartmentElements
        {
            get => _apartmentElements;
            set => Set(ref _apartmentElements, value);
        }

        private ObservableCollection<string> _panelCircuits;

        public ObservableCollection<string> PanelCircuits
        {
            get => _panelCircuits;
            set => Set(ref _panelCircuits, value);
        }

        private ObservableCollection<string> _circuitElements;

        public ObservableCollection<string> CircuitElements
        {
            get => _circuitElements;
            set => Set(ref _circuitElements, value);
        }

        public ICommand AddApartmentElement { get; set; }
        public ICommand EditApartmentElement { get; set; }
        public ICommand RemoveApartmentElement { get; set; }
        public ICommand AddPanelCircuit { get; set; }
        public ICommand EditPanelCircuit { get; set; }
        public ICommand RemovePanelCircuit { get; set; }
    }
}
