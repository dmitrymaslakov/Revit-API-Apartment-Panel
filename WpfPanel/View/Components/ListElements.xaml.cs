using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Models.RevitMockupModels;

namespace WpfPanel.View.Components
{
    /// <summary>
    /// Interaction logic for ListElements.xaml
    /// </summary>
    public partial class ListElements : Window
    {
        private const string TRISSA_SWITCH = "Trissa Switch";
        private const string USB = "USB";
        private const string BLOCK1 = "BLOCK1";
        private const string SINGLE_SOCKET = "Single Socket";
        private const string THROUGH_SWITCH = "Through Switch";

        private const string ELECTRICAL_FIXTURIES = "Electrical Fixturies";
        private const string LIGHTING_FIXTURIES = "Lighting Fixturies";
        private const string COMMUNICATION_DEVICES = "Communication Devices";

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
                new FamilySymbol{ Category = new Category { Name = ELECTRICAL_FIXTURIES}, Name = TRISSA_SWITCH},
                new FamilySymbol{ Category = new Category { Name = COMMUNICATION_DEVICES}, Name = USB},
                new FamilySymbol{ Category = new Category { Name = COMMUNICATION_DEVICES}, Name = BLOCK1},
                new FamilySymbol{ Category = new Category { Name = ELECTRICAL_FIXTURIES}, Name = SINGLE_SOCKET},
                new FamilySymbol{ Category = new Category { Name = LIGHTING_FIXTURIES}, Name = THROUGH_SWITCH},
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
