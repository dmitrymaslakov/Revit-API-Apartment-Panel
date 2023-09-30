using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfPanel.View
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI : Window
    {
        public UI(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
            Closing += UI_Closing;
        }

        private void UI_Closing(object sender, System.ComponentModel.CancelEventArgs e) 
            => latestConfig.Command?.Execute(null);

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            Button button = sender as Button;
            string currentCategory = button.Tag as string;
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
            currentSuffix.Command?.Execute(characterValue);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button.Tag.ToString() == "Lighting Devices")
                statusBarItem.Content = "You should select the lamp(s) before inserting the switch";
            button.Focus();
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) 
            => statusBarItem.Content = null;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in testLVOuter.Items)
            {
                
            }
        }
    }
}
