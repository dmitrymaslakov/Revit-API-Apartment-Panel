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

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /*public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register(nameof(Elements), typeof(ObservableCollection<string>),
                typeof(ElementsCard), new PropertyMetadata(null));

        public ObservableCollection<string> Elements
        {
            get { return (ObservableCollection<string>)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }*/

        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register(nameof(Elements), typeof(object),
                typeof(ElementsCard), new PropertyMetadata(null));

        public object Elements
        {
            get { return GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        public static readonly DependencyProperty SelectedElementsProperty =
            DependencyProperty.Register(nameof(SelectedElements), typeof(object),
                typeof(ElementsCard), new PropertyMetadata(null));

        public object SelectedElements
        {
            get { return GetValue(SelectedElementsProperty); }
            set { SetValue(SelectedElementsProperty, value); }
        }

        public static readonly DependencyProperty AddBtnVisibilityProperty =
            DependencyProperty.Register(nameof(AddBtnVisibility), typeof(Visibility),
                typeof(ElementsCard), new PropertyMetadata(Visibility.Visible));

        public string AddBtnVisibility
        {
            get { return (string)GetValue(AddBtnVisibilityProperty); }
            set { SetValue(AddBtnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty EditBtnVisibilityProperty =
            DependencyProperty.Register(nameof(EditBtnVisibility), typeof(Visibility),
                typeof(ElementsCard), new PropertyMetadata(Visibility.Visible));

        public string EditBtnVisibility
        {
            get { return (string)GetValue(EditBtnVisibilityProperty); }
            set { SetValue(EditBtnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty RemoveBtnVisibilityProperty =
            DependencyProperty.Register(nameof(RemoveBtnVisibility), typeof(Visibility),
                typeof(ElementsCard), new PropertyMetadata(Visibility.Visible));

        public string RemoveBtnVisibility
        {
            get { return (string)GetValue(RemoveBtnVisibilityProperty); }
            set { SetValue(RemoveBtnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty IsDictionaryKeysProperty =
            DependencyProperty.Register(nameof(IsDictionaryKeys), typeof(bool),
                typeof(ElementsCard), new PropertyMetadata(false));

        public bool IsDictionaryKeys
        {
            get { return (bool)GetValue(IsDictionaryKeysProperty); }
            set { SetValue(IsDictionaryKeysProperty, value); }
        }

        public static readonly DependencyProperty AddElementPropertyCommand =
            DependencyProperty.Register(nameof(AddElementCommand), typeof(ICommand),
                typeof(ElementsCard), new PropertyMetadata(null));

        public ICommand AddElementCommand
        {
            get { return (ICommand)GetValue(AddElementPropertyCommand); }
            set { SetValue(AddElementPropertyCommand, value); }
        }

        public static readonly DependencyProperty EditElementPropertyCommand =
            DependencyProperty.Register(nameof(EditElementCommand), typeof(ICommand),
                typeof(ElementsCard), new PropertyMetadata(null));

        public ICommand EditElementCommand
        {
            get { return (ICommand)GetValue(EditElementPropertyCommand); }
            set { SetValue(EditElementPropertyCommand, value); }
        }

        public static readonly DependencyProperty RemoveElementPropertyCommand =
            DependencyProperty.Register(nameof(RemoveElementCommand), typeof(ICommand),
                typeof(ElementsCard), new PropertyMetadata(null));

        public ICommand RemoveElementCommand
        {
            get { return (ICommand)GetValue(RemoveElementPropertyCommand); }
            set { SetValue(RemoveElementPropertyCommand, value); }
        }

        public static readonly DependencyProperty SelectedElementsPropertyCommand =
            DependencyProperty.Register(nameof(SelectedElementsCommand), typeof(ICommand),
                typeof(ElementsCard), new PropertyMetadata(null));

        public ICommand SelectedElementsCommand
        {
            get { return (ICommand)GetValue(SelectedElementsPropertyCommand); }
            set { SetValue(SelectedElementsPropertyCommand, value); }
        }

        public ElementsCard()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = ((TextBox)sender).Text;
            var si = lv.DisplayMemberPath;
            /*Header = t;
            SelectedElement = t;*/
        }

        private void Lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedElements = lv.SelectedItems;
            SelectedElementsCommand?.Execute(lv.SelectedItems);
        }
    }
}
