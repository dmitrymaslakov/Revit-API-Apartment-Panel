using ApartmentPanel.Core.Models.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for CircuitElements.xaml
    /// </summary>
    public partial class CircuitElements : UserControl
    {
        #region HeaderProperty
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(CircuitElements),
                new PropertyMetadata(string.Empty));
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region ElementsProperty
        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register(nameof(Elements), typeof(ObservableCollection<IApartmentElement>),
                typeof(CircuitElements), new PropertyMetadata(null));
        public ObservableCollection<IApartmentElement> Elements
        {
            get { return (ObservableCollection<IApartmentElement>)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }
        #endregion

        public static readonly DependencyProperty SelectElementsCommandProperty =
            DependencyProperty.Register(nameof(SelectElementsCommand), typeof(ICommand),
                typeof(CircuitElements), new PropertyMetadata(null));

        public ICommand SelectElementsCommand
        {
            get { return (ICommand)GetValue(SelectElementsCommandProperty); }
            set { SetValue(SelectElementsCommandProperty, value); }
        }

        public CircuitElements()
        {
            InitializeComponent();
        }

        private void Lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SelectedElements = lv.SelectedItems;
            SelectElementsCommand?.Execute(lv.SelectedItems);
        }

    }
}
