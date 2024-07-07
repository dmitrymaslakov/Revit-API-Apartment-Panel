using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM.ConfigPanelComponentsVM;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ApartmentPanel.Presentation.View.Components.ConfigPanelComponents
{
    /// <summary>
    /// Interaction logic for ApartmentElementsView.xaml
    /// </summary>
    public partial class ApartmentElementsView : UserControl
    {
        public ApartmentElementsView()
        {
            InitializeComponent();
        }

        private void Lv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IApartmentElement apartmentElement = (IApartmentElement)lv.SelectedItem;
            var selectedElements = new List<IApartmentElement> { apartmentElement };
            var aeVM = root.DataContext as ApartmentElementsViewModel;
            aeVM.SelectApartmentElementCommand?.Execute(selectedElements);
        }
    }
}
