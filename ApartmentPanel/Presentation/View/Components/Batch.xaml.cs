using ApartmentPanel.Core.Models.Batch;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        #region SelectedBatchedElementProperty
        public static readonly DependencyProperty SelectedBatchedElementProperty =
            DependencyProperty.Register(nameof(SelectedBatchedElement), typeof(BatchedElement),
                typeof(Batch), new PropertyMetadata(null));

        public BatchedElement SelectedBatchedElement
        {
            get { return (BatchedElement)GetValue(SelectedBatchedElementProperty); }
            set { SetValue(SelectedBatchedElementProperty, value); }
        }
        #endregion

        #region SelectedRowProperty
        public static readonly DependencyProperty SelectedRowProperty =
            DependencyProperty.Register(nameof(SelectedRow), typeof(BatchedRow),
                typeof(Batch), new PropertyMetadata(null));

        public BatchedRow SelectedRow
        {
            get { return (BatchedRow)GetValue(SelectedRowProperty); }
            set { SetValue(SelectedRowProperty, value); }
        }
        #endregion

        #region AddElementToRowCommandProperty
        public static readonly DependencyProperty AddElementToRowCommandProperty =
            DependencyProperty.Register(nameof(AddElementToRowCommand), typeof(ICommand),
                typeof(Batch), new PropertyMetadata(null));

        public ICommand AddElementToRowCommand
        {
            get { return (ICommand)GetValue(AddElementToRowCommandProperty); }
            set { SetValue(AddElementToRowCommandProperty, value); }
        }
        #endregion

        #region RemoveElementFromRowCommandProperty
        public static readonly DependencyProperty RemoveElementFromRowCommandProperty =
            DependencyProperty.Register(nameof(RemoveElementFromRowCommand), typeof(ICommand),
                typeof(Batch), new PropertyMetadata(null));

        public ICommand RemoveElementFromRowCommand
        {
            get { return (ICommand)GetValue(RemoveElementFromRowCommandProperty); }
            set { SetValue(RemoveElementFromRowCommandProperty, value); }
        }
        #endregion

        #region AddRowToBatchCommandProperty
        public static readonly DependencyProperty AddRowToBatchCommandProperty =
            DependencyProperty.Register(nameof(AddRowToBatchCommand), typeof(ICommand),
                typeof(Batch), new PropertyMetadata(null));

        public ICommand AddRowToBatchCommand
        {
            get { return (ICommand)GetValue(AddRowToBatchCommandProperty); }
            set { SetValue(AddRowToBatchCommandProperty, value); }
        }
        #endregion

        #region RemoveRowFromBatchCommandProperty
        public static readonly DependencyProperty RemoveRowFromBatchCommandProperty =
            DependencyProperty.Register(nameof(RemoveRowFromBatchCommand), typeof(ICommand),
                typeof(Batch), new PropertyMetadata(null));

        public ICommand RemoveRowFromBatchCommand
        {
            get { return (ICommand)GetValue(RemoveRowFromBatchCommandProperty); }
            set { SetValue(RemoveRowFromBatchCommandProperty, value); }
        }
        #endregion

        private BatchedRow FindParentRow(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !((parent as ContentPresenter)?.Content is BatchedRow))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return (parent as ContentPresenter).Content as BatchedRow;
        }

        private void Button_AddElementToRow(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            BatchedRow targetRow = FindParentRow(button);
            AddElementToRowCommand?.Execute(targetRow);
        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            BatchedRow targetRow = FindParentRow(button);
            RemoveElementFromRowCommand?.Execute(targetRow);
        }

        private void Button_AddRow(object sender, RoutedEventArgs e) => 
            AddRowToBatchCommand?.Execute(null);

        private void Button_RemoveRow(object sender, RoutedEventArgs e) => 
            RemoveRowFromBatchCommand?.Execute(null);

        private void ListView_SelectionElementChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBatchedRows.Count == 1)
                removeRowBtn.IsEnabled = false;
            else if (ListBatchedRows.Count > 1)
                removeRowBtn.IsEnabled = true;
        }

        private void ListView_GotFocus(object sender, RoutedEventArgs e)
        {
            ListViewItem listViewItem = e.OriginalSource as ListViewItem;
            if (listViewItem?.Content is BatchedElement batchedElement) 
                SelectedBatchedElement = batchedElement;
        }
    }
}
