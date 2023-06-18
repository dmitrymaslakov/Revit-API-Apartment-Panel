using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WpfPanel.ViewModel;
using WpfPanel.View;
using Autodesk.Revit.UI.Events;
using WpfPanel.Domain;

namespace WpfPanel
{
    public class WpfApp : IExternalApplication
    {
        internal static WpfApp thisApp = null;
        private UIViewModel _viewModel;
        private UI _ui;

        public Result OnStartup(UIControlledApplication application)
        {
            _viewModel = null;
            _ui = null;
            thisApp = this;
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            if (_ui != null && _viewModel != null)
            {
                _ui = null;
                _viewModel = null;

                application.Idling -= IdlingHandler;
            }

            return Result.Succeeded;
        }

        public void ShowForm(UIApplication uiapp)
        {
            // If we do not have a dialog yet, create and show it
            if (_ui == null)
            {
                _viewModel = new UIViewModel(uiapp);
                _ui = new UI(_viewModel);
                _ui.Show();

                // if we have a dialog, we need Idling too
                uiapp.Idling += IdlingHandler;
            }
        }

        public void IdlingHandler(object sender, IdlingEventArgs args)
        {
            UIApplication uiapp = sender as UIApplication;
            if (_ui == null || _viewModel == null)
            {
                uiapp.Idling -= IdlingHandler;
                return;
            }
            RequestId request = _viewModel.Request.Take();
            if (request != RequestId.None)
            {
                try
                {
                    RequestHandler.Execute(uiapp, request);
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Revit", ex.Message);
                }
            }
            return;
        }
    }


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Program : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                WpfApp.thisApp.ShowForm(commandData.Application);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
