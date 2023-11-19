using System.Windows;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for ElementsList.xaml
    /// </summary>
    public partial class ElementsList : Window
    {
        public ElementsList(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}
