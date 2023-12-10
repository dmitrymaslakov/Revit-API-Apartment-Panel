using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WpfTest.Models.Batch;

namespace WpfTest.Views.Components
{
    /// <summary>
    /// Interaction logic for Batch.xaml
    /// </summary>
    public partial class Batch : UserControl
    {
        public Batch() => InitializeComponent();

        public static readonly DependencyProperty ListBatchedRowsProperty =
            DependencyProperty.Register(nameof(ListBatchedRows), typeof(ObservableCollection<BatchedRow>),
                typeof(Batch), new PropertyMetadata(null));

        public ObservableCollection<BatchedRow> ListBatchedRows
        {
            get { return (ObservableCollection<BatchedRow>)GetValue(ListBatchedRowsProperty); }
            set { SetValue(ListBatchedRowsProperty, value); }
        }

    }
}
