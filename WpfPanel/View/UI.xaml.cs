using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            Button button = sender as Button;
            string currentCategory = button.Tag as string;
            string targetCategory = "Lighting Fixtures";

            if (!currentCategory.Contains(targetCategory))
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
            if (button.Tag.ToString() == "Lighting Fixtures")
                statusBarItem.Content = "You should select the lamp(s) before inserting the switch";
            button.Focus();
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            statusBarItem.Content = null;
        }
    }
}
