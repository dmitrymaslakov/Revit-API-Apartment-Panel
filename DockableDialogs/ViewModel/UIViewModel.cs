using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DockableDialogs.Domain;

namespace DockableDialogs.ViewModel
{
    public class UIViewModel : ViewModelBase
    {
        public UIViewModel(UIApplication uiapp)
        {
            PlacementFixturies = new PlacementFixturiesCommand(this);
        }

        public ICommand PlacementFixturies
        {
            get;
            set;
        }
    }
}
