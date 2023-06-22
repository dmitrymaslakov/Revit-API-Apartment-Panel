using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using DockableDialogs.Domain;
using DockableDialogs.Utility;
using DockableDialogs.View;
using DockableDialogs.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            {
                application.RegisterDockablePane(UI.PaneId,
                    UI.PaneName,
                    View);
            }
            return Result.Succeeded;
        }

        private void OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            if (e.Document == null)
                return;

            /*if (View.DataContext is UIViewModel viewModel)
            {
                viewModel.DocumentTitle = e.Document.Title;
            }*/
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
    /*public class Application : IExternalApplication
    {
        internal static Application thisApp = null;
        private UI myForm;

        public Result OnShutdown(UIControlledApplication application)
        {
            if (myForm != null)
            {
                myForm.Close();
            }

            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            myForm = null;   // no dialog needed yet; the command will bring it
            thisApp = this;  // static access to this application instance

            return Result.Succeeded;
        }

        public void ShowForm(UIApplication uiapp)
        {
            if (myForm == null)
            {
                RequestHandler handler = new RequestHandler();

                ExternalEvent exEvent = ExternalEvent.Create(handler);
                var viewModel = new UIViewModel(exEvent, handler);
                myForm = new UI(viewModel);
                myForm.Show();
            }
        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        public virtual Result Execute(ExternalCommandData commandData
            , ref string message, ElementSet elements)
        {
            try
            {
                Application.thisApp.ShowForm(commandData.Application);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }*/

}
