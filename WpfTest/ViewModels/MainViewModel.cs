using System.Linq;
using WpfTest.Models.Batch;
using System.Windows;
using System.Collections.ObjectModel;
using WpfTest.Models;
using System.Windows.Input;
using WpfTest.Commands;
using System;

namespace WpfTest.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        #region MockFields
        private static ObservableCollection<ApartmentElement> el1 = new ObservableCollection<ApartmentElement>
        {
            new ApartmentElement { Name = "Switch"},
            new ApartmentElement { Name = "Socket"},
            new ApartmentElement { Name = "ThroughSwitch"}
        };
        private static ObservableCollection<ApartmentElement> el2 = new ObservableCollection<ApartmentElement>
        {
            new ApartmentElement { Name = "Smoke Sensor"},
            new ApartmentElement { Name = "USB"},
            new ApartmentElement { Name = "ThroughSwitch"}
        };
        #endregion

        #region MockProps
        public ObservableCollection<Circuit> MockPanelCircuits { get; set; } = new ObservableCollection<Circuit>
        {
            new Circuit{ Number = "1", Elements = el1 },
            new Circuit{ Number = "2", Elements = el2 },
        };
        #endregion

        public MainViewModel()
        {
            int elementIndex = 1;
            var BatchedElements1 = Enumerable.Range(1, 3).Select(i => new BatchedElement
            {
                Name = $"elName {elementIndex++}",
                Margin = new Thickness(i, 0, 0, 0)
            });
            var rows = Enumerable.Range(1, 2).Select(i => new BatchedRow
            {
                Number = i,
                HeightFromFloor = new Models.Height(),
                RowElements = new ObservableCollection<BatchedElement>(BatchedElements1)
            });

            ElementsBatch = new ElementsBatch
            {
                BatchedRows = new ObservableCollection<BatchedRow>(rows)
            };
            AddElementToRowCommand = new RelayCommand(OnAddElementToRowCommandExecuted);
            CreateNewElementForBatchCommand = new RelayCommand(OnCreateNewElementForBatchCommandExecuted);
        }
        public ElementsBatch ElementsBatch { get; set; }

        private BatchedElement _newElementForBatch;
        public BatchedElement NewElementForBatch
        {
            get => _newElementForBatch;
            set => Set(ref _newElementForBatch, value);
        }

        public ICommand AddElementToRowCommand { get; }
        public ICommand CreateNewElementForBatchCommand { get; }
        private void OnAddElementToRowCommandExecuted(object o)
        {
            var row = (BatchedRow)o;
            row.RowElements.Add(NewElementForBatch);
        }

        private void OnCreateNewElementForBatchCommandExecuted(object o)
        {
            (string circuit, string elementName, string elementCategory) =
                (ValueTuple<string, string, string>)o;

            NewElementForBatch = new BatchedElement { Circuit = circuit, Name = elementName };
        }
    }
}
