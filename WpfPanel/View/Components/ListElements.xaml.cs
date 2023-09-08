using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Models.RevitMockupModels;
using WpfPanel.Utilities;

namespace WpfPanel.View.Components
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

        public ListElements(Action<FamilySymbol>  addElementToApartment)
        {
            _addElementToApartment = addElementToApartment;
            AllElements = new ObservableCollection<CategorizedFamilySymbols>(GetCategorizedElements());
            InitializeComponent();
        }
        
        private IEnumerable<CategorizedFamilySymbols> GetCategorizedElements()
        {
            var e = new List<FamilySymbol>
            {
                new FamilySymbol{ Category = new Category { Name = StaticData.ELECTRICAL_FIXTURES}, Name = StaticData.TRISSA_SWITCH},
                new FamilySymbol{ Category = new Category { Name = StaticData.COMMUNICATION_DEVICES}, Name = StaticData.USB},
                new FamilySymbol{ Category = new Category { Name = StaticData.COMMUNICATION_DEVICES}, Name = StaticData.BLOCK1},
                new FamilySymbol{ Category = new Category { Name = StaticData.ELECTRICAL_FIXTURES}, Name = StaticData.SINGLE_SOCKET},
                new FamilySymbol{ Category = new Category { Name = StaticData.LIGHTING_DEVICES}, Name = StaticData.THROUGH_SWITCH},
                new FamilySymbol{ Category = new Category { Name = StaticData.LIGHTING_FIXTURES}, Name = StaticData.LAMP},
            }
            .GroupBy(fs => fs.Category.Name)
            .Select(gfs => new CategorizedFamilySymbols
            {
                Category = gfs.Key,
                CategorizedElements =
                new ObservableCollection<FamilySymbol>(gfs.Select(fs => fs))
            }).ToList();
            return e;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var nv = e.NewValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (elementsTree.SelectedItem is FamilySymbol selectedElement)
            {
                _addElementToApartment(selectedElement);
            }
        }
    }
}
