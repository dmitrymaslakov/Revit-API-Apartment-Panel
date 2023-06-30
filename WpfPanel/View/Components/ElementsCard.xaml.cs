using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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

namespace WpfPanel.View.Components
{
    /// <summary>
    /// Interaction logic for PanelElements.xaml
    /// </summary>
    public partial class ElementsCard : UserControl
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(ElementsCard),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register(nameof(Elements), typeof(ObservableCollection<string>),
                typeof(ElementsCard), new PropertyMetadata(null));

        public static readonly DependencyProperty AddElementProperty =
            DependencyProperty.Register(nameof(AddElement), typeof(ICommand),
                typeof(ElementsCard), new PropertyMetadata(null));

        public static readonly DependencyProperty EditElementProperty = 
            DependencyProperty.Register(nameof(EditElement), typeof(ICommand),
                typeof(ElementsCard), new PropertyMetadata(null));

        public static readonly DependencyProperty RemoveElementProperty =
            DependencyProperty.Register(nameof(RemoveElement), typeof(ICommand),
                typeof(ElementsCard), new PropertyMetadata(null));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public ObservableCollection<string> Elements
        {
            get { return (ObservableCollection<string>)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        public ICommand AddElement
        {
            get { return (ICommand)GetValue(AddElementProperty); }
            set { SetValue(AddElementProperty, value); }
        }

        public ICommand EditElement
        {
            get { return (ICommand)GetValue(EditElementProperty); }
            set { SetValue(EditElementProperty, value); }
        }

        public ICommand RemoveElement
        {
            get { return (ICommand)GetValue(RemoveElementProperty); }
            set { SetValue(RemoveElementProperty, value); }
        }

        public ElementsCard()
        {
            InitializeComponent();
        }
    }
}
