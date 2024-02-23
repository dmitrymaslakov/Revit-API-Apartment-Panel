using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for ConfigList.xaml
    /// </summary>
    public partial class ConfigList : UserControl
    {
        #region HeaderProperty
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(ConfigList),
                new PropertyMetadata(string.Empty));
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region ElementsProperty
        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register(nameof(Elements), typeof(ObservableCollection<string>),
                typeof(ConfigList), new PropertyMetadata(null));
        public ObservableCollection<string> Elements
        {
            get { return (ObservableCollection<string>)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }
        #endregion

        #region SelectedElementProperty
        public static readonly DependencyProperty SelectedElementProperty =
            DependencyProperty.Register(nameof(SelectedElement), typeof(string),
                typeof(ConfigList), new PropertyMetadata(string.Empty));

        public string SelectedElement
        {
            get { return (string)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); }
        }
        #endregion

        public static readonly DependencyProperty SelectedElementsProperty =
            DependencyProperty.Register(nameof(SelectedElements), typeof(ObservableCollection<string>),
                typeof(ConfigList), new PropertyMetadata(null));
        public ObservableCollection<string> SelectedElements
        {
            get { return (ObservableCollection<string>)GetValue(SelectedElementsProperty); }
            set { SetValue(SelectedElementsProperty, value); }
        }

        public static readonly DependencyProperty AddBtnVisibilityProperty =
            DependencyProperty.Register(nameof(AddBtnVisibility), typeof(Visibility),
                typeof(ConfigList), new PropertyMetadata(Visibility.Visible));
        public string AddBtnVisibility
        {
            get { return (string)GetValue(AddBtnVisibilityProperty); }
            set { SetValue(AddBtnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty EditBtnVisibilityProperty =
            DependencyProperty.Register(nameof(EditBtnVisibility), typeof(Visibility),
                typeof(ConfigList), new PropertyMetadata(Visibility.Visible));
        public string EditBtnVisibility
        {
            get { return (string)GetValue(EditBtnVisibilityProperty); }
            set { SetValue(EditBtnVisibilityProperty, value); }
        }
        public static readonly DependencyProperty RemoveBtnVisibilityProperty =
            DependencyProperty.Register(nameof(RemoveBtnVisibility), typeof(Visibility),
                typeof(ConfigList), new PropertyMetadata(Visibility.Visible));

        public string RemoveBtnVisibility
        {
            get { return (string)GetValue(RemoveBtnVisibilityProperty); }
            set { SetValue(RemoveBtnVisibilityProperty, value); }
        }
        public static readonly DependencyProperty AddElementCommandProperty =
            DependencyProperty.Register(nameof(AddElementCommand), typeof(ICommand),
                typeof(ConfigList), new PropertyMetadata(null));
        public ICommand AddElementCommand
        {
            get { return (ICommand)GetValue(AddElementCommandProperty); }
            set { SetValue(AddElementCommandProperty, value); }
        }

        public static readonly DependencyProperty EditElementCommandProperty =
            DependencyProperty.Register(nameof(EditElementCommand), typeof(ICommand),
                typeof(ConfigList), new PropertyMetadata(null));
        public ICommand EditElementCommand
        {
            get { return (ICommand)GetValue(EditElementCommandProperty); }
            set { SetValue(EditElementCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveElementCommandProperty =
            DependencyProperty.Register(nameof(RemoveElementCommand), typeof(ICommand),
                typeof(ConfigList), new PropertyMetadata(null));
        public ICommand RemoveElementCommand
        {
            get { return (ICommand)GetValue(RemoveElementCommandProperty); }
            set { SetValue(RemoveElementCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectElementsCommandProperty =
            DependencyProperty.Register(nameof(SelectElementsCommand), typeof(ICommand),
                typeof(ConfigList), new PropertyMetadata(null));
        public ICommand SelectElementsCommand
        {
            get { return (ICommand)GetValue(SelectElementsCommandProperty); }
            set { SetValue(SelectElementsCommandProperty, value); }
        }

        public ConfigList() => InitializeComponent();

        private void Lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*SelectedElements = new ObservableCollection<string>(lv.SelectedItems.ToList());
            SelectElementsCommand?.Execute(lv.SelectedItems);*/
        }
    }
}
