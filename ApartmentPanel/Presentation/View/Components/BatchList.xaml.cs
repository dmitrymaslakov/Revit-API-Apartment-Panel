using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.Utility;
using System;
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

        public static readonly DependencyProperty KeyDownCommandProperty =
            DependencyProperty.Register(nameof(KeyDownCommand), typeof(ICommand),
                typeof(BatchList), new PropertyMetadata(null));
        public ICommand KeyDownCommand
        {
            get { return (ICommand)GetValue(KeyDownCommandProperty); }
            set { SetValue(KeyDownCommandProperty, value); }
        }

        private void ListView_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                /*Button button = sender as Button;
                ElementBatch batch = button.DataContext as ElementBatch;
                HitElementCommand?.Execute(batch);*/
                //ListViewItem listViewItem = e.OriginalSource as ListViewItem;
                if (!(e.OriginalSource is ListViewItem listViewItem)) return;

                ElementBatch batch = listViewItem?.Content as ElementBatch;
                HitElementCommand?.Execute(batch);
                root.Focusable = true;
                root.Focus();
            }
            catch (Exception)
            {

            }
        }

        /*private void Image_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownCommand?.Execute(e.Key);
        }*/

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (!image.Focusable)
                image.Focusable = true;
            image.Focus();
        }

        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    e.Handled = true;
                    break;
                default:
                    break;
            }
            KeyDownCommand?.Execute(e.Key);
        }
    }
}
