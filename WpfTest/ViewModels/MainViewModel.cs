using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfTest.Commands;
using WpfTest.MockData;
using WpfTest.Models;

namespace WpfTest.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            SetHeightCommand = new RelayCommand(OnSetHeightCommandExecuted);
            AddHeightCommand = new RelayCommand(OnAddHeightCommandExecuted);
            ListHeightsOK = new ObservableCollection<double>();//(HeightsList.HeightsOK);
            ListHeightsUK = new ObservableCollection<double>(HeightsList.HeightsUK);
            ListHeightsCenter = new ObservableCollection<double>(HeightsList.HeightsCenter);
            //ElementHeight = new Height();
        }

        private Height _elementHeight;

        public Height ElementHeight
        {
            get => _elementHeight;
            set => Set(ref _elementHeight, value);
        }

        #region listHeights
        private ObservableCollection<double> _listHeightsOK;

        public ObservableCollection<double> ListHeightsOK
        {
            get => _listHeightsOK;
            set => Set(ref _listHeightsOK, value);
        }

        private ObservableCollection<double> _listHeightsUK;

        public ObservableCollection<double> ListHeightsUK
        {
            get => _listHeightsUK;
            set => Set(ref _listHeightsUK, value);
        }

        private ObservableCollection<double> _listHeightsCenter;

        public ObservableCollection<double> ListHeightsCenter
        {
            get => _listHeightsCenter;
            set => Set(ref _listHeightsCenter, value);
        }
        #endregion

        #region SetHeightCommand
        public ICommand SetHeightCommand { get; set; }

        private void OnSetHeightCommandExecuted(object o)
        {
            (string typeOfHeight, double height) =
                (ValueTuple<string, double>)o;

            ElementHeight = new Height { TypeOf = typeOfHeight, Value = height };
        }
        #endregion

        #region AddHeightCommand
        public ICommand AddHeightCommand { get; set; }

        private void OnAddHeightCommandExecuted(object o)
        {
            double newHeight = (double)o;
        }
        #endregion

    }
}
