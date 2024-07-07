using ApartmentPanel.Presentation.ViewModel.ComponentsVM.ConfigPanelComponentsVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.View.Components.ConfigPanelComponents
{
    /// <summary>
    /// Interaction logic for PanelCircuitsView.xaml
    /// </summary>
    public partial class PanelCircuitsView : UserControl
    {
        public static readonly DependencyProperty AddBtnVisibilityProperty =
            DependencyProperty.Register(nameof(AddBtnVisibility), typeof(Visibility),
                typeof(PanelCircuitsView), new PropertyMetadata(Visibility.Visible));

        public string AddBtnVisibility
        {
            get { return (string)GetValue(AddBtnVisibilityProperty); }
            set { SetValue(AddBtnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty EditBtnVisibilityProperty =
            DependencyProperty.Register(nameof(EditBtnVisibility), typeof(Visibility),
                typeof(PanelCircuitsView), new PropertyMetadata(Visibility.Visible));

        public string EditBtnVisibility
        {
            get { return (string)GetValue(EditBtnVisibilityProperty); }
            set { SetValue(EditBtnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty RemoveBtnVisibilityProperty =
            DependencyProperty.Register(nameof(RemoveBtnVisibility), typeof(Visibility),
                typeof(PanelCircuitsView), new PropertyMetadata(Visibility.Visible));

        public string RemoveBtnVisibility
        {
            get { return (string)GetValue(RemoveBtnVisibilityProperty); }
            set { SetValue(RemoveBtnVisibilityProperty, value); }
        }

        public static readonly DependencyProperty AddElementCommandProperty =
            DependencyProperty.Register(nameof(AddElementCommand), typeof(ICommand),
                typeof(PanelCircuitsView), new PropertyMetadata(null));

        public ICommand AddElementCommand
        {
            get { return (ICommand)GetValue(AddElementCommandProperty); }
            set { SetValue(AddElementCommandProperty, value); }
        }

        public static readonly DependencyProperty EditElementCommandProperty =
            DependencyProperty.Register(nameof(EditElementCommand), typeof(ICommand),
                typeof(PanelCircuitsView), new PropertyMetadata(null));

        public ICommand EditElementCommand
        {
            get { return (ICommand)GetValue(EditElementCommandProperty); }
            set { SetValue(EditElementCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveElementCommandProperty =
            DependencyProperty.Register(nameof(RemoveElementCommand), typeof(ICommand),
                typeof(PanelCircuitsView), new PropertyMetadata(null));

        public ICommand RemoveElementCommand
        {
            get { return (ICommand)GetValue(RemoveElementCommandProperty); }
            set { SetValue(RemoveElementCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectElementsCommandProperty =
            DependencyProperty.Register(nameof(SelectElementsCommand), typeof(ICommand),
                typeof(PanelCircuitsView), new PropertyMetadata(null));

        public ICommand SelectElementsCommand
        {
            get { return (ICommand)GetValue(SelectElementsCommandProperty); }
            set { SetValue(SelectElementsCommandProperty, value); }
        }

        public PanelCircuitsView() => InitializeComponent();

        private void Lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pc = root.DataContext as PanelCircuitsViewModel;
            pc.SelectPanelCircuitCommand?.Execute(lv.SelectedItems);
        }
    }
}
