using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DockableDialogs.ViewModel;

namespace DockableDialogs.Domain
{
    public class PlacementFixturiesCommand : BaseCommand
    {
        private readonly Action<object> _execute;

        public PlacementFixturiesCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
