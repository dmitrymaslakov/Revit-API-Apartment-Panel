using ApartmentPanel.Presentation.Models.Batch;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for Batch.xaml
    /// </summary>
    public partial class Batch : UserControl
    {
        public Batch() => InitializeComponent();

        #region ListBatchedRowsProperty
        public static readonly DependencyProperty ListBatchedRowsProperty =
            DependencyProperty.Register(nameof(ListBatchedRows), typeof(ObservableCollection<BatchedRow>),
                typeof(Batch), new PropertyMetadata(null));

        public ObservableCollection<BatchedRow> ListBatchedRows
        {
            get { return (ObservableCollection<BatchedRow>)GetValue(ListBatchedRowsProperty); }
            set { SetValue(ListBatchedRowsProperty, value); }
        }
        #endregion

        #region BatchAnnotationProperty
        public static readonly DependencyProperty BatchAnnotationProperty =
            DependencyProperty.Register(nameof(BatchAnnotation), typeof(BitmapSource),
                typeof(Batch), new PropertyMetadata(null));

        public BitmapSource BatchAnnotation
        {
            get { return (BitmapSource)GetValue(BatchAnnotationProperty); }
            set { SetValue(BatchAnnotationProperty, value); }
        }
        #endregion

        #region BatchNameProperty
        public static readonly DependencyProperty BatchNameProperty =
            DependencyProperty.Register(nameof(BatchName), typeof(string),
                typeof(Batch), new PropertyMetadata(string.Empty));

        public string BatchName
        {
            get { return (string)GetValue(BatchNameProperty); }
            set { SetValue(BatchNameProperty, value); }
        }
        #endregion

        public static readonly DependencyProperty AddElementToRowCommandProperty =
            DependencyProperty.Register(nameof(AddElementToRowCommand), typeof(ICommand),
                typeof(Batch), new PropertyMetadata(null));

        public ICommand AddElementToRowCommand
        {
            get { return (ICommand)GetValue(AddElementToRowCommandProperty); }
            set { SetValue(AddElementToRowCommandProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            BatchedRow targetRow = FindParentRow(button);
            AddElementToRowCommand?.Execute(targetRow);
        }

        private BatchedRow FindParentRow(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !((parent as ContentPresenter)?.Content is BatchedRow))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return (parent as ContentPresenter).Content as BatchedRow;
        }

    }
}
