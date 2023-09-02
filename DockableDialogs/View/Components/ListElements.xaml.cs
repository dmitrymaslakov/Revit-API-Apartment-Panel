using Autodesk.Revit.DB;
using DockableDialogs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DockableDialogs.View.Components
{
    /// <summary>
    /// Interaction logic for ListElements.xaml
    /// </summary>
    public partial class ListElements : Window
    {
        private readonly Action<FamilySymbol> _addElementToApartment;

        public static readonly DependencyProperty AllElementsProperty =
            DependencyProperty.Register(nameof(AllElements), typeof(ObservableCollection<CategorizedFamilySymbols>),
                typeof(ElementsCard), new PropertyMetadata(null));

        public ObservableCollection<CategorizedFamilySymbols> AllElements
        {
            get { return (ObservableCollection<CategorizedFamilySymbols>)GetValue(AllElementsProperty); }
            set { SetValue(AllElementsProperty, value); }
        }

        public ListElements(Action<FamilySymbol> addElementToApartment, List<FamilySymbol> allElements)
        {
            _addElementToApartment = addElementToApartment;
            AllElements = 
                new ObservableCollection<CategorizedFamilySymbols>(GetCategorizedElements(allElements));
            InitializeComponent();
        }

        private IEnumerable<CategorizedFamilySymbols> GetCategorizedElements(List<FamilySymbol> allElements)
        {
            return allElements
            .GroupBy(fs => fs.Category.Name)
            .Select(gfs => new CategorizedFamilySymbols
            {
                Category = gfs.Key,
                CategorizedElements =
                new ObservableCollection<FamilySymbol>(gfs.Select(fs => fs))
            }).ToList();
       }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var nv = e.NewValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (elementsTree.SelectedItem is FamilySymbol selectedElement)
                _addElementToApartment(selectedElement);
        }
    }
}
