using System.Windows;

namespace WpfPanel.View.Components
{
    /// <summary>
    /// Interaction logic for EditPanel.xaml
    /// </summary>
    public partial class EditPanel : Window
    {
        public EditPanel(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = PanelCircuitsCard.SelectedElements;
        }
    }
}
