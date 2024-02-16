using ApartmentPanel.Core.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for HeightList.xaml
    /// </summary>
    public partial class HeightList : UserControl
    {
        public HeightList()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TypeOfHeightProperty =
            DependencyProperty.Register(nameof(TypeOfHeight), typeof(TypeOfHeight), typeof(HeightList),
                new PropertyMetadata(TypeOfHeight.UK));

        public TypeOfHeight TypeOfHeight
        {
            get { return (TypeOfHeight)GetValue(TypeOfHeightProperty); }
            set { SetValue(TypeOfHeightProperty, value); }
        }

        public static readonly DependencyProperty ListHeightsProperty =
            DependencyProperty.Register(nameof(ListHeights), typeof(ObservableCollection<double>),
                typeof(HeightList), new PropertyMetadata(null));

        public ObservableCollection<double> ListHeights
        {
            get { return (ObservableCollection<double>)GetValue(ListHeightsProperty); }
            set { SetValue(ListHeightsProperty, value); }
        }

        public static readonly DependencyProperty AddHeightBtnVisibilityProperty =
            DependencyProperty.Register(nameof(AddHeightBtnVisibility), typeof(Visibility),
                typeof(HeightList), new PropertyMetadata(Visibility.Visible));
        public Visibility AddHeightBtnVisibility
        {
            get { return (Visibility)GetValue(AddHeightBtnVisibilityProperty); }
            set { SetValue(AddHeightBtnVisibilityProperty, value); }
        }

        #region ActionWithSelectedHeightCommand
        public static readonly DependencyProperty ActionWithSelectedHeightCommandProperty =
            DependencyProperty.Register(nameof(ActionWithSelectedHeightCommand), typeof(ICommand),
                typeof(HeightList), new PropertyMetadata(null));

        public ICommand ActionWithSelectedHeightCommand
        {
            get { return (ICommand)GetValue(ActionWithSelectedHeightCommandProperty); }
            set { SetValue(ActionWithSelectedHeightCommandProperty, value); }
        }
        #endregion

        #region ResetSelectionProperty
        public static readonly DependencyProperty ResetSelectionProperty =
            DependencyProperty.Register(nameof(ResetSelection), typeof(bool),
                typeof(HeightList), new PropertyMetadata(false, OnResetSelectionChanged));
        public bool ResetSelection
        {
            get { return (bool)GetValue(ResetSelectionProperty); }
            set { SetValue(ResetSelectionProperty, value); }
        }
        private static void OnResetSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var heightList = d as HeightList;
            if (heightList == null) return;

            if (heightList.ResetSelection)
                heightList.heightsListLV.SelectedItems.Clear();
        }
        #endregion

        private void ListHeights_GotFocus(object sender, RoutedEventArgs e)
        {
            ListView listHeights = sender as ListView;
            TypeOfHeight typeOfHeight = (TypeOfHeight)((GridView)listHeights.View).Columns[0].Header;
            ListViewItem listViewItem = e.OriginalSource as ListViewItem;
            bool b = double.TryParse(listViewItem?.Content?.ToString(), out double height);
            if (b) ActionWithSelectedHeightCommand?.Execute((typeOfHeight, height));
        }

        private void AddNewHeight_Click(object sender, RoutedEventArgs e)
        {
            bool b = double.TryParse(newHeight.Text, out double height);

            if (b && !ListHeights.Contains(height))
                ListHeights.Add(height);
        }

        private void RemoveHeight_Click(object sender, RoutedEventArgs e)
        {
            bool b = double.TryParse(heightsListLV.SelectedItem?.ToString(), out double height);

            if (b) ListHeights.Remove(height);
        }
    }
}
