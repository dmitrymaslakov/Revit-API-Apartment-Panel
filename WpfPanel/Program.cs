using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WpfPanel.ViewModel;
using WpfPanel.View;
using WpfPanel.Domain;
using System.Windows;

namespace WpfPanel
{
    public class Program : Application
    {
        [STAThread]
        public static void Main()
        {
            var app = new Program();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            var ui = new UI(new UIViewModel());
            ui.Show();
        }
    }
}
