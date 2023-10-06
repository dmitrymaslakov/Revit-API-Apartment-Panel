using System;
using WpfPanel.ViewModel;
using WpfPanel.View;
using WpfPanel.Domain;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace WpfPanel
{
    public class Program : Application
    {
        private static IHost _host;
        [STAThread]
        public static void Main()
        {
            _host = CreateHostBuilder().Build();
            var app = new Program();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            RequestHandler handler = new RequestHandler();
            ExternalEvent exEvent = ExternalEvent.Create(handler);
            var uiVM = new UIViewModel(exEvent, handler);
            var ui = new UI(uiVM);
            ui.Show();
        }

        private static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args);
        }

    }
}
