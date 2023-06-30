using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using DockableDialogs.Domain;
using DockableDialogs.Utility;
using DockableDialogs.View;
using DockableDialogs.ViewModel;
using System;
using WPF = System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DockableDialogs
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        public static UI View;
        internal static Application _thisApp = null;

        public Application()
        {
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Add UI for registering, showing, and hiding dockable panes.
        /// </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            application.ViewActivated += OnViewActivated;
            CreateWindow();

            if (!DockablePane.PaneIsRegistered(UI.PaneId))
                application.RegisterDockablePane(UI.PaneId, UI.PaneName, View);

             return Result.Succeeded;
        }

        private void OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            if (e.Document == null)
                return;
        }
        /// <summary>
        /// Create the new WPF Page that Revit will dock.
        /// </summary>
        public void CreateWindow()
        {
            RequestHandler handler = new RequestHandler();
            ExternalEvent exEvent = ExternalEvent.Create(handler);
            var uiVM = new UIViewModel(exEvent, handler);
            View = new UI(uiVM);
        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        public virtual Result Execute(ExternalCommandData commandData, 
            ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;

            if (DockablePane.PaneIsRegistered(UI.PaneId))
            {
                DockablePane myCustomPane =
                    uiapp.GetDockablePane(UI.PaneId);
                myCustomPane.Show();
            }
            else
            {
                return Result.Failed;
            }
            return Result.Succeeded;
        }
    }
    
    /*public class Application : WPF.Application
    {
        [STAThread]
        public static void Main()
        {
            var app = new Application();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var ui = new UI(new UIViewModel());
            ui.Show();
        }
    }*/
}
