using System.Windows;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for ElementList.xaml
    /// </summary>
    public partial class ElementList : Window
    {
        public ElementList(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}
