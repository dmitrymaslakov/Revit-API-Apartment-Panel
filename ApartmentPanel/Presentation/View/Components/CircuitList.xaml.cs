using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for CircuitList.xaml
    /// </summary>
    public partial class CircuitList : UserControl
    {
        public static readonly DependencyProperty CircuitsProperty =
            DependencyProperty.Register(nameof(Circuits), typeof(ObservableCollection<Circuit>),
                typeof(CircuitList), new PropertyMetadata(null));
        public ObservableCollection<Circuit> Circuits
        {
            get { return (ObservableCollection<Circuit>)GetValue(CircuitsProperty); }
            set { SetValue(CircuitsProperty, value); }
        }

        public static readonly DependencyProperty HitElementCommandProperty =
            DependencyProperty.Register(nameof(HitElementCommand), typeof(ICommand), 
                typeof(CircuitList), new PropertyMetadata(null));
        public ICommand HitElementCommand
        {
            get { return (ICommand)GetValue(HitElementCommandProperty); }
            set { SetValue(HitElementCommandProperty, value); }
        }

        public static readonly DependencyProperty KeyDownCommandProperty =
            DependencyProperty.Register(nameof(KeyDownCommand), typeof(ICommand),
                typeof(CircuitList), new PropertyMetadata(null));
        public ICommand KeyDownCommand
        {
            get { return (ICommand)GetValue(KeyDownCommandProperty); }
            set { SetValue(KeyDownCommandProperty, value); }
        }

        public static readonly DependencyProperty MouseEnterCommandProperty =
            DependencyProperty.Register(nameof(MouseEnterCommand), typeof(ICommand),
                typeof(CircuitList), new PropertyMetadata(null));
        public ICommand MouseEnterCommand
        {
            get { return (ICommand)GetValue(MouseEnterCommandProperty); }
            set { SetValue(MouseEnterCommandProperty, value); }
        }

        public static readonly DependencyProperty MouseLeaveCommandProperty =
            DependencyProperty.Register(nameof(MouseLeaveCommand), typeof(ICommand),
                typeof(CircuitList), new PropertyMetadata(null));
        public ICommand MouseLeaveCommand
        {
            get { return (ICommand)GetValue(MouseLeaveCommandProperty); }
            set { SetValue(MouseLeaveCommandProperty, value); }
        }

        public CircuitList()
        {
            InitializeComponent();
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            Button button = sender as Button;
            IApartmentElement element = button.DataContext as IApartmentElement;

            string currentCategory = element.Category;
            string targetCategory = "Lighting Fixtures";

            if (!currentCategory.Contains(targetCategory)
                || e.Key == Key.LeftCtrl
                || e.Key == Key.RightCtrl)
                return;

            string characterValue = "";
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                char numericChar = (char)('0' + (e.Key - Key.D0));
                characterValue = numericChar.ToString();
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                char numericChar = (char)('0' + (e.Key - Key.NumPad0));
                characterValue = numericChar.ToString();
            }
            KeyDownCommand?.Execute(characterValue);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            IApartmentElement element = button.DataContext as IApartmentElement;

            if (element.Category == "Lighting Devices")
                MouseEnterCommand?.Execute("MouseEnter");
            button.Focus();
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) => 
            MouseLeaveCommand?.Execute("MouseLeave");

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            IApartmentElement element = button.DataContext as IApartmentElement;
            Circuit circuit = FindParentCircuit(button);

            string category = element.Category;
            string name = element.Name;
            ImageSource annotation = element.Annotation;
            string circuitNumber = circuit?.Number;

            HitElementCommand?.Execute((circuitNumber, name, category, annotation));
        }

        private Circuit FindParentCircuit(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !((parent as ContentPresenter)?.Content is Circuit))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return (parent as ContentPresenter).Content as Circuit;
        }
    }
}
