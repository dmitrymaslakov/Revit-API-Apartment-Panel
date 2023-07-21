using System;
using System.Windows;

namespace WpfPanel.View.Components
{
    /// <summary>
    /// Interaction logic for EditPanel.xaml
    /// </summary>
    public partial class EditPanel : Window
    {
        public EditPanel(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
            OkBtn.CommandParameter = new Action(Confige.Close);
            /*ApplyBtn.CommandParameter = new Action(Confige.Close);
            CancelBtn.CommandParameter = new Action(Confige.Close);*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var d = "Dima";
            var t = d;
            t = "qwe";
        }
    }
}
