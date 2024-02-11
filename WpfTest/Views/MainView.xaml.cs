using System.Windows;
using System.Windows.Controls;

namespace WpfTest.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }

        private void list1_GotFocus(object sender, RoutedEventArgs e)
        {
            ListView listHeights = sender as ListView;
            var si = listHeights.SelectedItem;
            var os = e.OriginalSource as ListViewItem;
            
            MessageBox.Show($"GotFocus - {e.OriginalSource}");
        }

        private void list1_LostFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"LostFocus - {e.OriginalSource}");
        }
    }
}
