using System.Windows;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for ListElements.xaml
    /// </summary>
    public partial class ListElements : Window
    {
        public ListElements(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}
