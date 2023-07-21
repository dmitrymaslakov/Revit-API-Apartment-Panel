using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Services.Commands;
using WpfPanel.Utilities;
using WpfPanel.View.Components;
using WpfPanel.ViewModel.ComponentsVM;

namespace WpfPanel.ViewModel
{
    public enum OkApplyCancel
    {
        Ok, Apply, Cancel
    }

    public class UIViewModel : ViewModelBase
    {
        private readonly EditPanelVM _editPanelVM;
        private readonly Action<object, OkApplyCancel> _okApplyCancelActions;

        public UIViewModel(ExternalEvent exEvent, RequestHandler handler)
            : base(exEvent, handler)
        {
            /*_handler = handler;
            _exEvent = exEvent;*/

            _editPanelVM = new EditPanelVM(exEvent, handler, ExecuteOkApplyCancelActions);
            _editPanelVM.TestProp = "TestProp";
            Circuits = GetCircuits(_editPanelVM.PanelCircuits);
            /*Circuits = new ObservableCollection<Circuit>
            {
                new Circuit { Number = 1 },
                new Circuit { Number = 2 },
                new Circuit { Number = 3 },
            };*/
            Configure = new ConfigureCommand(o =>
            {
                _handler.Props = _editPanelVM;
                _handler.Request.Make(RequestId.Configure);
                _exEvent.Raise();
            });

            InsertElementCommand = new RelayCommand(o =>
            {
                var t = o as Button;
                var b = t.Tag;
                var e = t.Content;
            });
            //_okApplyCancelActions = ExecuteOkApplyCancelActions;
        }

        private void ExecuteOkApplyCancelActions(object obj, OkApplyCancel okApplyCancel)
        {
            switch (okApplyCancel)
            {
                case OkApplyCancel.Ok:
                case OkApplyCancel.Apply:
                    Circuits.Clear();
                    var panelCircuits =
                        (ObservableDictionary<string, ObservableCollection<string>>)obj;
                    Circuits = GetCircuits(panelCircuits);
                    break;
                case OkApplyCancel.Cancel:
                    break;
            }
        }

        private ObservableCollection<Circuit> _circuits;

        public ObservableCollection<Circuit> Circuits
        {
            get => _circuits;
            set => Set(ref _circuits, value);
        }

        public ICommand Configure { get; set; }

        public ICommand InsertElementCommand { get; set; }

        private ObservableCollection<Circuit> GetCircuits(
            ObservableDictionary<string, ObservableCollection<string>> panelCircuits)
        {
            var result = new ObservableCollection<Circuit>();
            foreach (var circuit in panelCircuits)
            {
                result.Add(new Circuit
                {
                    Number = circuit.Key,
                    //ApartmentElements = new ObservableCollection<string>(circuit.Value.Select(e => e).ToList())
                    ApartmentElements = new ObservableCollection<string>(circuit.Value.ToList())
                }) ;
            }
            return result;
        }
    }
}
