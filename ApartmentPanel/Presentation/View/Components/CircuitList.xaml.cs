using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Utility;
using System;
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

        public static readonly DependencyProperty AnnotationLikeButtonProperty =
            DependencyProperty.Register(nameof(AnnotationLikeButton), typeof(bool),
                typeof(CircuitList), new PropertyMetadata(false));
        public bool AnnotationLikeButton
        {
            get { return (bool)GetValue(AnnotationLikeButtonProperty); }
            set { SetValue(AnnotationLikeButtonProperty, value); }
        }

        public static readonly DependencyProperty HitElementCommandProperty =
            DependencyProperty.Register(nameof(HitElementCommand), typeof(ICommand),
                typeof(CircuitList), new PropertyMetadata(null));
        public ICommand HitElementCommand
        {
            get { return (ICommand)GetValue(HitElementCommandProperty); }
            set { SetValue(HitElementCommandProperty, value); }
        }

        public static readonly DependencyProperty CharKeyDownCommandProperty =
            DependencyProperty.Register(nameof(CharKeyDownCommand), typeof(ICommand),
                typeof(CircuitList), new PropertyMetadata(null));
        public ICommand CharKeyDownCommand
        {
            get { return (ICommand)GetValue(CharKeyDownCommandProperty); }
            set { SetValue(CharKeyDownCommandProperty, value); }
        }

        public static readonly DependencyProperty NumberKeyDownCommandProperty =
            DependencyProperty.Register(nameof(NumberKeyDownCommand), typeof(ICommand),
                typeof(CircuitList), new PropertyMetadata(null));
        public ICommand NumberKeyDownCommand
        {
            get { return (ICommand)GetValue(NumberKeyDownCommandProperty); }
            set { SetValue(NumberKeyDownCommandProperty, value); }
        }

        public static readonly DependencyProperty ArrowKeyDownCommandProperty =
            DependencyProperty.Register(nameof(ArrowKeyDownCommand), typeof(ICommand),
                typeof(CircuitList), new PropertyMetadata(null));
        public ICommand ArrowKeyDownCommand
        {
            get { return (ICommand)GetValue(ArrowKeyDownCommandProperty); }
            set { SetValue(ArrowKeyDownCommandProperty, value); }
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

        private void Image_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                /*Image image = sender as Image;
                IApartmentElement element = image.DataContext as IApartmentElement;

                string currentCategory = element.Category;
                string targetCategory = "Lighting Fixtures";

                if (!currentCategory.Contains(targetCategory)
                    || e.Key == Key.LeftCtrl
                    || e.Key == Key.RightCtrl)
                    return;*/
                /*string characterValue = "";
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
                else if (e.Key >= Key.A && e.Key <= Key.Z)
                {
                    if (char.TryParse(e.Key.ToString(), out char parsedCharacter))
                        characterValue = parsedCharacter.ToString().ToLower();
                }*/

                /*string characterValue = KeyToStringParser.ParseNumber(e.Key);
                if (string.IsNullOrEmpty(characterValue))
                    characterValue = KeyToStringParser.ParseChar(e.Key);*/
                CharKeyDownCommand?.Execute(e.Key);
            }
            catch (Exception)
            {

            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Image image = sender as Image;
                /*IApartmentElement element = image.DataContext as IApartmentElement;
                if (element.Category == "Lighting Devices")
                    MouseEnterCommand?.Execute("MouseEnter");*/
                if(!image.Focusable) image.Focusable = true;
                image.Focus();
                MouseEnterCommand?.Execute("MouseEnter");
            }
            catch (Exception)
            {
                return;
            }

        }

        private void Image_MouseLeave(object sender, MouseEventArgs e) =>
            MouseLeaveCommand?.Execute("MouseLeave");

        private void ListView_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ListView lv = sender as ListView;
                if (!(e.OriginalSource is ListViewItem lvi)) return;

                IApartmentElement element = lvi.Content as IApartmentElement;
                Circuit circuit = lv.DataContext as Circuit;

                string circuitNumber = circuit?.Number;

                HitElementCommand?.Execute((circuitNumber, element));
                if(AnnotationLikeButton) lvp.Focus();
            }
            catch (Exception)
            {
                return;
            }
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

        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    e.Handled = true;
                    break;
                default:
                    break;
            }
            ArrowKeyDownCommand?.Execute(e.Key);
        }
    }
}
