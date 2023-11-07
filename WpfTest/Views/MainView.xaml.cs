using System.Windows;

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
    }
}
