using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Services.Commands;
using WpfPanel.Utilities;
using WpfPanel.View.Components;
using WpfPanel.ViewModel.ComponentsVM;
using static System.Net.Mime.MediaTypeNames;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

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
            ConfigureCommand = new ConfigureCommand(o =>
            {
                _handler.Props = _editPanelVM;
                _handler.Request.Make(RequestId.Configure);
                _exEvent.Raise();
            });

            InsertElementCommand = new RelayCommand(o =>
            {
                (string circuit, string elementName, string elementCategory) =
                    (ValueTuple<string, string, string>)o;

                _handler.Props = new Dictionary<string, string>
                {
                    { nameof(circuit), circuit },
                    { nameof(elementName), elementName },
                    { nameof(elementCategory), elementCategory },
                    { "lampSuffix", CurrentSuffix },
                    { "height", Height.ToString() },
                };
                _handler.Request.Make(RequestId.Insert);
                _exEvent.Raise();
            });

            SetCurrentSuffixCommand = new RelayCommand(o => CurrentSuffix = o as string);

            Height = 40.0;
        }

        private void ExecuteOkApplyCancelActions(object obj, OkApplyCancel okApplyCancel)
        {
            switch (okApplyCancel)
            {
                case OkApplyCancel.Ok:
                case OkApplyCancel.Apply:
                    Circuits.Clear();
                    var panelCircuits =
                        (ObservableDictionary<string, ObservableCollection<ApartmentElement>>)obj;
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

        private string _currentSuffix;

        public string CurrentSuffix
        {
            get => _currentSuffix;
            set => Set(ref _currentSuffix, value);
        }
        
        private double _height;

        public double Height
        {
            get => _height;
            set => Set(ref _height, value);
        }

        private string _status;

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        public ICommand ConfigureCommand { get; set; }

        public ICommand InsertElementCommand { get; set; }

        public ICommand SetCurrentSuffixCommand { get; set; }

        private ObservableCollection<Circuit> GetCircuits(
            ObservableDictionary<string, ObservableCollection<ApartmentElement>> panelCircuits)
        {
            var result = new ObservableCollection<Circuit>();
            foreach (var circuit in panelCircuits)
            {
                result.Add(new Circuit
                {
                    Number = circuit.Key,
                    //ApartmentElements = new ObservableCollection<string>(circuit.Value.Select(e => e).ToList())
                    ApartmentElements =
                        new ObservableCollection<ApartmentElement>(circuit.Value.ToList())
                });
            }
            return result;
        }
    }
}
