using ApartmentPanel.Presentation.ViewModel.ComponentsVM.ConfigPanelComponentsVM;
using System.Windows.Controls;

namespace ApartmentPanel.Presentation.View.Components.ConfigPanelComponents
{
    /// <summary>
    /// Interaction logic for CircuitElementsView.xaml
    /// </summary>
    public partial class CircuitElementsView : UserControl
    {
        public CircuitElementsView() => InitializeComponent();

        private void Lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ce = root.DataContext as CircuitElementsViewModel;
            ce.SelectCircuitElementCommand?.Execute(lv.SelectedItems);
        }
    }
}
