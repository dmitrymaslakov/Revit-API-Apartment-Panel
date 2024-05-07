using ApartmentPanel.Utility.Extensions.RevitExtensions;
using ApartmentPanel.Utility.SelectionFilters;
//using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;

namespace ApartmentPanel.Infrastructure.Handler.HandlerStates
{
    internal class AnalyzingHandlerState : HandlerState
    {
        internal override void Handle(UIApplication uiapp)
        {
            SetInfrastructure(uiapp);
            var a = GetAngleBetweenBasisXAxisAndCurrentXAxis();
        }

        private double GetAngleBetweenBasisXAxisAndCurrentXAxis()
        {
            Transform identity = Transform.Identity;
            var origin = identity.Origin;
            XYZ localXAxis = identity.BasisX;
            XYZ globalXAxis = XYZ.BasisX;
            return globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisX);
        }

    }
}
