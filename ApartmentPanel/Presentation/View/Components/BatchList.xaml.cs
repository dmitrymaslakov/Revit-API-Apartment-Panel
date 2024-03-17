using ApartmentPanel.Presentation.Models.Batch;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for BatchList.xaml
    /// </summary>
    public partial class BatchList : UserControl
    {
        public BatchList() => InitializeComponent();

        #region BatchesProperty
        public static readonly DependencyProperty BatchesProperty =
            DependencyProperty.Register(nameof(Batches), typeof(ObservableCollection<ElementBatch>),
                typeof(BatchList), new PropertyMetadata(null));
        public ObservableCollection<ElementBatch> Batches
        {
            get { return (ObservableCollection<ElementBatch>)GetValue(BatchesProperty); }
            set { SetValue(BatchesProperty, value); }
        }
        #endregion

        #region HitElementCommandProperty
        public static readonly DependencyProperty HitElementCommandProperty =
            DependencyProperty.Register(nameof(HitElementCommand), typeof(ICommand),
                typeof(BatchList), new PropertyMetadata(null));
        public ICommand HitElementCommand
        {
            get { return (ICommand)GetValue(HitElementCommandProperty); }
            set { SetValue(HitElementCommandProperty, value); }
        }
        #endregion

        private void ListView_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
            /*Button button = sender as Button;
            ElementBatch batch = button.DataContext as ElementBatch;
            HitElementCommand?.Execute(batch);*/
                ListViewItem listViewItem = e.OriginalSource as ListViewItem;
                ElementBatch batch = listViewItem?.Content as ElementBatch;
                HitElementCommand?.Execute(batch);
                root.Focusable = true;
                root.Focus();
            }
            catch (System.Exception)
            {

            }
        }
    }
}
