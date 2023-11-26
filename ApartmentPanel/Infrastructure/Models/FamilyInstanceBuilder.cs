using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ApartmentPanel.Utility.Exceptions;
using ApartmentPanel.Utility;
using ApartmentPanel.Infrastructure.Models.LocationStrategies;
using ApartmentPanel.Core.Models;

namespace ApartmentPanel.Infrastructure.Models
{
    public class FamilyInstanceBuilder
    {
        #region Private fields
        private UIApplication _uiapp;
        private UIDocument _uiDocument;
        private Document _document;
        private Selection _selection;
        /*private double _elevationFromLevel;
        private string _height;*/
        private ILocationStrategy _locationStrategy;
        private ElementId _currentLevelId;
        private string _circuit;
        private string _lampSuffix;
        private string _switchNumbers;

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
        public FamilyInstanceBuilder WithHeight(Height height, ILocationStrategy locationStrategy)
        {
            _locationStrategy = locationStrategy;
            /*_height = height;
            if (!string.IsNullOrEmpty(height))
            {
                string elevationFromLevel = height.Split('=')[1];
                bool isElevationParsed = double.TryParse(elevationFromLevel,
                    out _elevationFromLevel);
            }*/
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
        public FamilyInstanceBuilder WithSwitchNumbers(string switchNumbers)
        {
            _switchNumbers = switchNumbers;
            return this;
        }
        #endregion

        #region Private methods
        private ElementId FamilyInstanceCreate(FamilySymbol symbol)
        {
            Reference reference =
                _selection.PickObject(ObjectType.PointOnElement, "Pick a host in the model");

            FamilyInstance newFamilyInstance = null;
            using (var tr = new Transaction(_document, "Creating new FamilyInstance"))
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
            View active = _document.ActiveView;
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
        
        /*private void SetElevationFromLevel(FamilyInstance familyInstance)
        {
            double newElevationFeets = UnitUtils.ConvertToInternalUnits(_elevationFromLevel,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            familyInstance
                .get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM)
                .Set(newElevationFeets);
        }*/
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
                    circuitParam.Set(_circuit + "/" + _switchNumbers);
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
                    circuitParam.Set(_height);
                    break;
            }
        }
        #endregion
    }
}
