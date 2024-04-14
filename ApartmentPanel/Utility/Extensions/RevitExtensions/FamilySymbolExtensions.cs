using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ApartmentPanel.Utility.Extensions.RevitExtensions
{
    public static class FamilySymbolExtensions
    {
        public static bool CanBePlacedOnVerticalFace(
            this FamilySymbol familySymbol)
        {
            var parameters = familySymbol.Family.Parameters;

            var faceParameter = familySymbol.Family.Parameters
                .OfType<Parameter>()
                .FirstOrDefault(p => p.Definition.Name == "Host" && p.AsValueString() == "Face");

            return faceParameter != null;
        }
    }
}
