using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Diagnostics;
using System.Linq;
using Utility.Extensions;

namespace RevitTest
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AnalizeCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            AnalizeElement(commandData.Application);
            return Result.Succeeded;
        }
        private void AnalizeElement(UIApplication uiapp)
        {
            UIApplication _uiapp = uiapp;
            UIDocument _uiDocument = _uiapp.ActiveUIDocument;
            Document _document = _uiDocument.Document;
            Selection _selection = _uiDocument.Selection;

            try
            {
                //double angle = 90;
                XYZ translation = new XYZ(0.328084, 0, 0);
                var selectedElementId = _selection.GetElementIds().FirstOrDefault();
                var familyInstance = _document.GetElement(selectedElementId) as FamilyInstance;
                Transform instanceTransform = familyInstance.GetTransform();
                XYZ localXAxis = instanceTransform.BasisX;
                XYZ globalXAxis = XYZ.BasisX;
                double angle = globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisZ);

                Transform rotation = Transform.CreateRotation(XYZ.BasisZ, angle);
                XYZ rotatedVector = rotation.OfVector(translation);
                using (var tr = new Transaction(_document, "Translation of object"))
                {
                    tr.Start();
                    familyInstance.Location.Move(rotatedVector);
                    tr.Commit();
                }
                /*Debug.Write($"translationOrigin - {translationOrigin}");
                Debug.Write($"transBasisX - {transBasisX}");
                Debug.Write($"transBasisY - {transBasisY}");
                Debug.Write($"transBasisZ - {transBasisZ}");
                Debug.Write($"-------------------------------------------------");
                Debug.Write($"rotationOrigin - {rotationOrigin}");
                Debug.Write($"rotBasisX - {rotBasisX}");
                Debug.Write($"rotBasisY - {rotBasisY}");
                Debug.Write($"rotBasisZ - {rotBasisZ}");*/

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Analize_Exception", ex.Message);
            }
        }
        private double ToRadians(double degrees)
            => degrees * Math.PI / 180;
        private double ToDegrees(double radians)
            => radians * 180 / Math.PI;

    }
}
