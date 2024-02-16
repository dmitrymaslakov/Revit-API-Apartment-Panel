using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for ElementParameters.xaml
    /// </summary>
    public partial class ElementParameters : UserControl
    {
        #region HeaderProperty
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(ElementParameters),
                new PropertyMetadata(string.Empty));
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region ParametersProperty
        public static readonly DependencyProperty ParametersProperty =
            DependencyProperty.Register(nameof(Parameters), typeof(ObservableCollection<Parameter>),
                typeof(ElementParameters), new PropertyMetadata(null));
        public ObservableCollection<Parameter> Parameters
        {
            get { return (ObservableCollection<Parameter>)GetValue(ParametersProperty); }
            set { SetValue(ParametersProperty, value); }
        }
        #endregion

        public ElementParameters()
        {
            InitializeComponent();
        }
    }
}
