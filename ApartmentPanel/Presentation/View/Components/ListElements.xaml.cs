using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Core.Services.AnnotationService;
using System.Windows.Media;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for ListElements.xaml
    /// </summary>
    public partial class ListElements : Window
    {
        //private readonly Action<IApartmentElement> _addElementToApartment;
        //private readonly IConfigPanelViewModel _configPanelProperties;

        /*public static readonly DependencyProperty AllElementsProperty =
            DependencyProperty.Register(nameof(AllElements), 
                //typeof(ObservableCollection<CategorizedFamilySymbols>),
                typeof(ObservableCollection<IApartmentElement>),
                typeof(ElementsCard), new PropertyMetadata(null));*/

        //public ObservableCollection<CategorizedFamilySymbols> AllElements
        /*public ObservableCollection<IApartmentElement> AllElements
        {
            //get { return (ObservableCollection<CategorizedFamilySymbols>)GetValue(AllElementsProperty); }
            get { return (ObservableCollection<IApartmentElement>)GetValue(AllElementsProperty); }
            set { SetValue(AllElementsProperty, value); }
        }*/

        //public ListElements(Action<IApartmentElement> addElementToApartment, 
        public ListElements(object dataContext)
            /*(IConfigPanelViewModel configPanelProperties,
            List<IApartmentElement> categorizedElements)*/
        {
            //_addElementToApartment = addElementToApartment;
            //_configPanelProperties = configPanelProperties;
            //AllElements =
            //new ObservableCollection<CategorizedFamilySymbols>(categorizedElements);
            //new ObservableCollection<IApartmentElement>(categorizedElements);
            DataContext = dataContext;
            InitializeComponent();
        }

        /*private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var nv = e.NewValue;
        }*/

        /*private void AddElementToApartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (elementsTree.SelectedItem is IApartmentElement selectedElement)
                //_addElementToApartment(selectedElement);
                {
                    if (!_configPanelProperties.ApartmentElements.Select(ae => ae.Name).Contains(selectedElement.Name))
                    {
                        var annotationService = new AnnotationService(
                            new FileAnnotationCommunicatorFactory(selectedElement.Name));

                        ImageSource annotation = annotationService.IsAnnotationExists()
                            ? annotationService.Get() : null;

                        IApartmentElement newApartmentElement = selectedElement.Clone();
                        newApartmentElement.Annotation = annotation;
                        _configPanelProperties.ApartmentElements.Add(newApartmentElement);
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("exeption", ex.Message);
            }
        }*/
    }
}
