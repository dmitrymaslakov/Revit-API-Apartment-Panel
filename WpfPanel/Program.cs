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

namespace WpfPanel
{
    public class WpfApp : IExternalApplication
    {
        internal static WpfApp thisApp = null;
        private UI myForm;

        public Result OnStartup(UIControlledApplication application)
        {
            myForm = null;   // no dialog needed yet; the command will bring it
            thisApp = this;
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            if (myForm != null && !myForm.IsDisposed)
            {
                myForm.Dispose();
                myForm = null;

                // if we've had a dialog, we had subscribed
                application.Idling -= IdlingHandler;
            }

            return Result.Succeeded;
        }

        public void ShowForm(UIApplication uiapp)
        {
            // If we do not have a dialog yet, create and show it
            if (myForm == null)
            {
                var viewModel = new UIViewModel();
                myForm = new UI(viewModel);
                myForm.ShowDialog();

                // if we have a dialog, we need Idling too
                uiapp.Idling += IdlingHandler;
            }
        }

        public void IdlingHandler(object sender, IdlingEventArgs args)
        {
            UIApplication uiapp = sender as UIApplication;

            if (myForm.IsDisposed)
            {
                uiapp.Idling -= IdlingHandler;
                return;
            }
            else   // dialog still exists
            {
                // fetch the request from the dialog

                RequestId request = myForm.Request.Take();

                if (request != RequestId.None)
                {
                    try
                    {
                        // we take the request, if any was made,
                        // and pass it on to the request executor

                        RequestHandler.Execute(uiapp, request);
                    }
                    finally
                    {
                        // The dialog may be in its waiting state;
                        // make sure we wake it up even if we get an exception.

                        m_MyForm.WakeUp();
                    }
                }
            }

            return;
        }

    }


    [Transaction(TransactionMode.Manual)]
    public class Program : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var viewModel = new UIViewModel();
            var ui = new UI(viewModel);
            ui.ShowDialog();
            return Result.Succeeded;
        }
    }
}
