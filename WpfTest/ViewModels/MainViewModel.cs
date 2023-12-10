using System.Linq;
using WpfTest.Models.Batch;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfTest.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
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
                RowElements = new List<BatchedElement>(BatchedElements1)
            });

            ElementsBatch = new ElementsBatch
            {
                BatchedRows = new ObservableCollection<BatchedRow>(rows)
            };
        }
        public ElementsBatch ElementsBatch { get; set; }
    }
}
