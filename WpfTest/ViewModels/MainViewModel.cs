using System.Linq;
using WpfTest.Models.Batch;
using System.Windows;
using System.Collections.ObjectModel;
using WpfTest.Models;
using System.Windows.Input;
using WpfTest.Commands;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Security.Policy;
using System.Collections.Generic;

namespace WpfTest.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        #region MockFields
        #endregion

        #region MockProps
        #endregion

        public MainViewModel()
        {
        }
        public ObservableCollection<string> List1 { get; set; } = new ObservableCollection<string>
        (
            Enumerable.Range(1, 3).Select(i => $"String-{i}")
        );
        public ObservableCollection<string> List2 { get; set; } = new ObservableCollection<string>
(
    Enumerable.Range(1, 3).Select(i => $"OtherString-{i}")
);

    }
}
