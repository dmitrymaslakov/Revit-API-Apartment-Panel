using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ApartmentPanel.Utility.Exceptions;

namespace ApartmentPanel.Utility
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
        public FamilyInstanceBuilder WithSwitchSuffixes(List<FamilyInstance> lamps)
        {
            _switchSuffixes = GetSuffixesFromLamp(lamps);
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

            using (var tr = new Transaction(_document, "Config FamilyInstance"))
            {
                tr.Start();
                XYZ dir = new XYZ(0, 0, 0);
                string category = familyInstance.Category.Name;
                if (_currentLevelId != null)
                {
                    SetCurrentLevel(familyInstance);
                    if (!category.Contains(StaticData.LIGHTING_FIXTURES))
                    {
                        SetElevationFromLevel(familyInstance);
                        try
                        {
                            SetHeight(familyInstance);
                        }
                        catch (CustomParameterException ex)
                        {
                            TaskDialog.Show($"Exception", ex.Message);
                        }
                    }
                }
                try
                {
                    SetCircuit(familyInstance);
                }
                catch (CustomParameterException ex)
                {
                    TaskDialog.Show($"Exception", ex.Message);
                }

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
            string customParameter = StaticData.ELEMENT_CIRCUIT_PARAM_NAME;
            Parameter circuitParam = familyInstance.LookupParameter(customParameter);
            if (circuitParam == null)
                throw new CustomParameterException(customParameter, familyInstance.Name);

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
            string customParameter = StaticData.ELEMENT_HEIGHT_PARAM_NAME;
            Parameter circuitParam = familyInstance.LookupParameter(customParameter);
            if (circuitParam == null)
                throw new CustomParameterException(customParameter, familyInstance.Name);

            string category = familyInstance.Category.Name;
            switch (category)
            {
                case StaticData.LIGHTING_DEVICES:
                case StaticData.ELECTRICAL_FIXTURES:
                case StaticData.TELEPHONE_DEVICES:
                case StaticData.FIRE_ALARM_DEVICES:
                case StaticData.COMMUNICATION_DEVICES:
                    circuitParam.Set($"H={_elevationFromLevel}");
                    break;
            }
        }
        private string GetSuffixesFromLamp(List<FamilyInstance> lamps)
        {
            var circuitParameters = new List<Parameter>();

            string targetCircuitParam = StaticData.ELEMENT_CIRCUIT_PARAM_NAME;

            foreach (var lamp in lamps)
            {
                var lampParameter = lamp.Parameters
                    .OfType<Parameter>()
                    .Where(p => p.Definition.Name.Contains(targetCircuitParam))
                    .FirstOrDefault();

                if (lampParameter == null)
                    throw new CustomParameterException(targetCircuitParam, lamp.Name);

                circuitParameters.Add(lampParameter);
            }

            var lampCircuits = new List<string>();

            foreach (Parameter parameter in circuitParameters)
                if (parameter.Definition.Name.Contains(targetCircuitParam))
                    if (parameter.StorageType == StorageType.String)
                        lampCircuits.Add(parameter.AsString());

            var suffixes = lampCircuits
                .Select(c => c.Substring(c.IndexOf("/") + 1))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return string.Join(",", suffixes);
        }
        #endregion
    }
}
