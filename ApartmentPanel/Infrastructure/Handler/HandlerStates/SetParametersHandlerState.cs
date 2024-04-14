using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ApartmentPanel.Infrastructure.Handler.HandlerStates
{
    internal class SetParametersHandlerState : HandlerState
    {
        internal override void Handle(UIApplication uiapp)
        {
            SetInfrastructure(uiapp);
            using (var tr = new Transaction(_document, "Set Parameters"))
            {
                tr.Start();
                SetParameters();
                tr.Commit();
            }
        }
        private void SetParameters()
        {
            var setParamsDTO = _handler.Props as SetParamsDTO;

            bool isInstanceExist =
                TryGetInstance(_document, setParamsDTO.ElementName, out FamilyInstance element);
            Wall tempWall = null;
            FamilyInstance tempElement = null;
            if (!isInstanceExist)
            {
                var ru = new RevitUtility(_uiapp);
                tempWall = ru.CreateWall();
                var symbol = new FilteredElementCollector(_document)
                .OfClass(typeof(FamilySymbol))
                .Where(fs => fs.Name == setParamsDTO.ElementName)
                .FirstOrDefault() as FamilySymbol;
                tempElement = ru.CreateFamilyInstance(symbol, tempWall);
            }

            List<string> parameters = new List<string>();
            ParameterSet parameterSet = element != null
                ? element.Parameters
                : tempElement.Parameters;

            foreach (Parameter param in parameterSet)
            {
                if (param == null) continue;
                parameters.Add(param.Definition.Name);
            }
            setParamsDTO.SetInstanceParameters(parameters);
            if (tempWall != null && tempElement != null)
            {
                List<ElementId> deletedElementIds = new List<ElementId>
                    {
                        tempWall.Id, tempElement.Id
                    };
                _document.Delete(deletedElementIds);
            }
            _handler.Props = null;
        }
        private bool TryGetInstance(Document document, string elementName, out FamilyInstance element)
        {
            element = new FilteredElementCollector(document)
                .OfClass(typeof(FamilyInstance))
                .ToElements()
                .Select(e => e as FamilyInstance)
                .FirstOrDefault(fi => fi.Name.Contains(elementName));
            return !(element is null);
        }
    }
}
