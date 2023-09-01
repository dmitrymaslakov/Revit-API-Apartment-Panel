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
using System.Text.Json;
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

    public class UIViewModel : ViewModelBase, IUIToCommandsCreater
    {
        private readonly UICommandsCreater _uICommandsCreater;

        public UIViewModel(ExternalEvent exEvent, RequestHandler handler)
            : base(exEvent, handler)
        {
            _uICommandsCreater = new UICommandsCreater(this);

            EditPanelVM = new EditPanelVM(exEvent, handler, ExecuteOkApplyCancelActions);

            LatestConfigPath = Path.Combine(Environment.CurrentDirectory, "LatestConfig.json");

            LoadLatestConfigCommand = _uICommandsCreater.CreateLoadLatestConfigCommand();

            LoadLatestConfigCommand.Execute(null);

            Circuits = GetCircuits(EditPanelVM.PanelCircuits);

            ConfigureCommand = _uICommandsCreater.CreateConfigureCommand();

            InsertElementCommand = _uICommandsCreater.CreateInsertElementCommand();

            SetCurrentSuffixCommand = _uICommandsCreater.CreateSetCurrentSuffixCommand();

            SaveLatestConfigCommand = _uICommandsCreater.CreateSaveLatestConfigCommand();

            Height = 40.0;
        }

        public string LatestConfigPath { get; }

        public EditPanelVM EditPanelVM { get; }

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

        public ICommand SaveLatestConfigCommand { get; set; }

        public ICommand LoadLatestConfigCommand { get; set; }

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

        private ObservableCollection<Circuit> GetCircuits(
            ObservableDictionary<string, ObservableCollection<ApartmentElement>> panelCircuits)
        {
            var result = new ObservableCollection<Circuit>();
            foreach (var circuit in panelCircuits)
            {

                var a = circuit.Value.Select(ap => ap.Clone()).ToList();

                foreach (var item in a)
                {
                    var ann = item.Annotation;
                }

                result.Add(new Circuit
                {
                    Number = circuit.Key,
                    ApartmentElements = new ObservableCollection<ApartmentElement>(
                            circuit.Value.Select(ap => ap.Clone()).ToList())
                });
            }
            return result;
        }
    }
}
