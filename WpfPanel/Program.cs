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

namespace WpfPanel
{
    /*public class WpfController : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }

    }*/


    [Transaction(TransactionMode.Manual)]
    public class Program : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var viewModel = new UIViewModel();
            var ui = new View.UI(viewModel);
            ui.ShowDialog();
            return Result.Succeeded;
        }
    }
}
