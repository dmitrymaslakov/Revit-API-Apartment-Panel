using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace DockableDialogs.Utility
{
    public class FamilyInstanceBuilder
    {
        #region Private fields
        private UIApplication _uiapp;
        private UIDocument _uiDocument;
        private Document _document;
        private Selection _selection;
        private double _elevationFromLevel;
        private ElementId _currentLevelId;
        private string _circuit;
        private string _lampSuffix;
        private string _switchSuffixes;

        #endregion

        public FamilyInstanceBuilder(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _uiDocument = _uiapp.ActiveUIDocument;
            _document = _uiDocument.Document;
            _selection = _uiDocument.Selection;
        }
        public ElementId Build(string elementName)
        {
            var familySymbol = new FilteredElementCollector(_document)
                .OfClass(typeof(FamilySymbol))
                .Where(fs => fs.Name == elementName)
                .FirstOrDefault() as FamilySymbol;
            ElementId familyInstanceId = FamilyInstanceCreate(familySymbol);
            FamilyInstanceConfigure(familyInstanceId);
            return familyInstanceId;
        }

        #region Configuration methods
        public FamilyInstanceBuilder WithElevationFromLevel(string elevationFromLevel)
        {
            bool isElevationParsed = double.TryParse(elevationFromLevel,
                out _elevationFromLevel);
            return this;
        }
        public FamilyInstanceBuilder WithCurrentLevel()
        {
            _currentLevelId = GetViewLevel();
            return this;
        }
        public FamilyInstanceBuilder WithCircuit(string circuit)
        {
            _circuit = circuit;
            return this;
        }
        public FamilyInstanceBuilder WithLampSuffix(string lampSuffix)
        {
            _lampSuffix = lampSuffix;
            return this;
        }
        public FamilyInstanceBuilder WithSwitchSuffixes()
        {
            _switchSuffixes = GetSuffixesFromLamps();
            return this;
        }
        #endregion

        #region Private methods
        private ElementId FamilyInstanceCreate(FamilySymbol symbol)
        {
            Reference reference = 
                _selection.PickObject(ObjectType.PointOnElement, "Pick a host in the model");

            FamilyInstance newFamilyInstance = null;
            using (var tr = new Transaction(_document, "Creating new FamilySymbol"))
            {
                tr.Start();
                XYZ dir = new XYZ(0, 0, 0);
                newFamilyInstance = _uiDocument
                    .Document.Create
                    .NewFamilyInstance(reference, reference.GlobalPoint, dir, symbol);
                tr.Commit();
            }
            return newFamilyInstance.Id;
        }
        private void FamilyInstanceConfigure(ElementId familyInstanceId)
        {
            FamilyInstance familyInstance = _document.GetElement(familyInstanceId) as FamilyInstance;
            //ElementId levelId = GetViewLevel();

            //string circuit = _elementData[nameof(circuit)];
            //string elementName = elementData[nameof(elementName)];
            //string elementCategory = elementData[nameof(elementCategory)];
            //string lampSuffix = _elementData[nameof(lampSuffix)];
            //string switchSuffixes = "";
            /*bool isElevationParsed = double.TryParse(elementData["elevationFromLevel"],
                out double elevationFromLevel);*/
            /*if (familyInstance.Category.Name.Contains(LIGHTING_DEVICES))
                switchSuffixes = elementData[nameof(switchSuffixes)];*/

            using (var tr = new Transaction(_document, "Config FamilyInstance"))
            {
                tr.Start();
                XYZ dir = new XYZ(0, 0, 0);
                string category = familyInstance.Category.Name;
                if (_currentLevelId != null)
                {
                    SetCurrentLevel(familyInstance);
                    /*familyInstance
                        .get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM)
                        .Set(_currentLevelId);*/
                    if (!category.Contains(StaticData.LIGHTING_FIXTURES))
                    {
                        SetElevationFromLevel(familyInstance);
                        /*double newElevationFeets = UnitUtils.ConvertToInternalUnits(_elevationFromLevel,
                            _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

                        familyInstance
                            .get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM)
                            .Set(newElevationFeets);*/
                    }
                }
                SetCircuit(familyInstance);
                /*Parameter circuitParam = familyInstance.LookupParameter("RBX-CIRCUIT");
                switch (category)
                {
                    case LIGHTING_FIXTURES:
                        circuitParam.Set(circuit + "/" + lampSuffix);
                        break;
                    case LIGHTING_DEVICES:
                        circuitParam.Set(circuit + "/" + switchSuffixes);
                        break;
                    case ELECTRICAL_FIXTURES:
                    case TELEPHONE_DEVICES:
                    case FIRE_ALARM_DEVICES:
                    case COMMUNICATION_DEVICES:
                        circuitParam.Set(circuit);
                        break;
                }*/
                tr.Commit();
            }
        }
        private ElementId GetViewLevel()
        {
            Autodesk.Revit.DB.View active = _document.ActiveView;
            ElementId levelId = null;
            Parameter level = active.LookupParameter("Associated Level");
            if (level == null)
                return null;

            FilteredElementCollector lvlCollector = new FilteredElementCollector(_document);
            ICollection<Element> lvlCollection = lvlCollector.OfClass(typeof(Level)).ToElements();
            foreach (Element l in lvlCollection)
            {
                Level lvl = l as Level;
                if (lvl.Name == level.AsString())
                    levelId = lvl.Id;
            }
            return levelId;
        }
        private void SetCurrentLevel(FamilyInstance familyInstance)
        {
            familyInstance
                .get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM)
                .Set(_currentLevelId);
        }
        private void SetElevationFromLevel(FamilyInstance familyInstance)
        {
            double newElevationFeets = UnitUtils.ConvertToInternalUnits(_elevationFromLevel,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            familyInstance
                .get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM)
                .Set(newElevationFeets);
        }
        private void SetCircuit(FamilyInstance familyInstance)
        {
            Parameter circuitParam = familyInstance.LookupParameter("RBX-CIRCUIT");
            string category = familyInstance.Category.Name;
            switch (category)
            {
                case StaticData.LIGHTING_FIXTURES:
                    circuitParam.Set(_circuit + "/" + _lampSuffix);
                    break;
                case StaticData.LIGHTING_DEVICES:
                    circuitParam.Set(_circuit + "/" + _switchSuffixes);
                    break;
                case StaticData.ELECTRICAL_FIXTURES:
                case StaticData.TELEPHONE_DEVICES:
                case StaticData.FIRE_ALARM_DEVICES:
                case StaticData.COMMUNICATION_DEVICES:
                    circuitParam.Set(_circuit);
                    break;
            }
        }
        private void SetHeight(FamilyInstance familyInstance)
        {
            Parameter circuitParam = familyInstance.LookupParameter("UK-HEIGHT");
            string category = familyInstance.Category.Name;
            switch (category)
            {
                case StaticData.LIGHTING_FIXTURES:
                    circuitParam.Set(_circuit + "/" + _lampSuffix);
                    break;
                case StaticData.LIGHTING_DEVICES:
                    circuitParam.Set(_circuit + "/" + _switchSuffixes);
                    break;
                case StaticData.ELECTRICAL_FIXTURES:
                case StaticData.TELEPHONE_DEVICES:
                case StaticData.FIRE_ALARM_DEVICES:
                case StaticData.COMMUNICATION_DEVICES:
                    circuitParam.Set(_circuit);
                    break;
            }
        }
        private string GetSuffixesFromLamps()
        {
            //try
            {
                var collection = _selection.GetElementIds()
                    .Select(id => _document.GetElement(id))
                    .OfType<FamilyInstance>()
                    .Where(fs => fs.Category.Name.Contains("Lighting Fixtures"))
                    ;
                if (collection.Count() < 1)
                {
                    string message =
                        "Please select the lamp(s) of Lighting Fixtures category before inserting the switch.";
                    throw new Exception(message);
                }

                var circuitParameters = new List<Parameter>();

                string targetCircuitParam = "RBX-CIRCUIT";

                foreach (var lamp in collection)
                {
                    var lampParameter = lamp.Parameters
                        .OfType<Parameter>()
                        .Where(p => p.Definition.Name.Contains(targetCircuitParam))
                        .FirstOrDefault();

                    if (lampParameter == null)
                    {
                        string message =
                            "Some of the Lighting Fixtures do not have RBX-CIRCUIT parameter.";
                        throw new Exception(message);
                    }

                    circuitParameters.Add(lampParameter);
                }

                var lampCircuits = new List<string>();

                foreach (Parameter parameter in circuitParameters)
                    if (parameter.Definition.Name.Contains(targetCircuitParam))
                        if (parameter.StorageType == StorageType.String)
                            lampCircuits.Add(parameter.AsString());

                var suffixes = lampCircuits
                    .Select(c => c.Substring(c.IndexOf("/") + 1))
                    .ToList();

                return string.Join(",", suffixes);

            }
            /*catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
                return null;
            }*/
        }

        #endregion
    }
}
