using MediatR;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Windows;
using WpfTest.ViewModels;
using WpfTest.Views;

namespace WpfTest
{
    internal class Program : Application
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
            _host.Start();
            var mainViewModel = new MainViewModel();
            new MainView(mainViewModel).Show();
        }
        private static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(s => s.AddMediatR(Assembly.GetExecutingAssembly()));
        }
    }
}
