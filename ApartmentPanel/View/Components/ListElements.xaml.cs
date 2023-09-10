using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ApartmentPanel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ApartmentPanel.View.Components
{
    /// <summary>
    /// Interaction logic for ListElements.xaml
    /// </summary>
    public partial class ListElements : Window
    {
        private readonly Action<ApartmentElement> _addElementToApartment;

        public static readonly DependencyProperty AllElementsProperty =
            DependencyProperty.Register(nameof(AllElements), typeof(ObservableCollection<CategorizedFamilySymbols>),
                typeof(ElementsCard), new PropertyMetadata(null));

        public ObservableCollection<CategorizedFamilySymbols> AllElements
        {
            get { return (ObservableCollection<CategorizedFamilySymbols>)GetValue(AllElementsProperty); }
            set { SetValue(AllElementsProperty, value); }
        }

        public ListElements(Action<ApartmentElement> addElementToApartment, IEnumerable<CategorizedFamilySymbols> categorizedElements)
        {
            _addElementToApartment = addElementToApartment;
            AllElements =
                new ObservableCollection<CategorizedFamilySymbols>(categorizedElements);
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var nv = e.NewValue;
        }

        private void AddElementToApartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (elementsTree.SelectedItem is ApartmentElement selectedElement)
                    _addElementToApartment(selectedElement);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("exeption", ex.Message);
            }
        }
    }
}
