using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register(nameof(SelectedElement), typeof(string),
                typeof(ElementsCard), new PropertyMetadata(string.Empty));

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

        public string SelectedElement
        {
            get { return (string)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }

        public ElementsCard()
        {
            InitializeComponent();
        }
    }
}
