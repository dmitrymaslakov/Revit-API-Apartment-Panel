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
        public UIViewModel UIViewModel { get; set; }

        public PlacementFixturiesCommand(UIViewModel uIViewModel)
        {
            UIViewModel = uIViewModel;
        }

        public override void Execute(object parameter)
        {
            ExternalEvent.Create(new ExternalEventHandler()).Raise();
        }
    }
}
