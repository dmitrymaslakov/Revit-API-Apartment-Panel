using System;
using System.Windows;
using WpfTest.ViewModels;
using WpfTest.Views;

namespace WpfTest
{
    internal class Program : Application
    {
        [STAThread]
        public static void Main()
        {
            var app = new Program();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            var mainViewModel = new MainViewModel();
            new MainView(mainViewModel).Show();
        }
    }
}
