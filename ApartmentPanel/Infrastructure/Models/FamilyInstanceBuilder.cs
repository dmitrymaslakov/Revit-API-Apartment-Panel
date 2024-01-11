using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ApartmentPanel.Utility.Exceptions;
using ApartmentPanel.Utility;
using ApartmentPanel.Infrastructure.Models.LocationStrategies;
using ApartmentPanel.Core.Models;
using System;
using Microsoft.Extensions.Hosting;

namespace ApartmentPanel.Infrastructure.Models
{
    public class FamilyInstanceBuilder : RevitInfrastructureBase
    {
        #region Private fields
        /*private UIApplication _uiapp;
        private UIDocument _uiDocument;
        private Document _document;
        private Selection _selection;*/
        /*private double _elevationFromLevel;*/
        private double _height;
        private string _renderedHeight;
        private ILocationStrategy _locationStrategy;
        private ElementId _currentLevelId;
        private string _circuit;
        private string _lampSuffix;
        private string _switchNumbers;
        //private double _offset;

        #endregion

        public FamilyInstanceBuilder(UIApplication uiapp) : base(uiapp) { }

        public Reference Host { get; private set; }

        public ElementId Build(string elementName)
        {
            var familySymbol = new FilteredElementCollector(_document)
                .OfClass(typeof(FamilySymbol))
                .Where(fs => fs.Name == elementName)
                .FirstOrDefault() as FamilySymbol;

            if (Host == null)
                Host = PickHost();

            ElementId familyInstanceId = FamilyInstanceCreate(familySymbol);
            FamilyInstanceConfigure(familyInstanceId);
            return familyInstanceId;
        }

        #region Configuration methods
        public FamilyInstanceBuilder SetLocationStrategy(ILocationStrategy locationStrategy)
        {
            _locationStrategy = locationStrategy;
            return this;
        }
        public FamilyInstanceBuilder RenderHeightAs(Func<double, string> heightFormat)
        {
            _renderedHeight = heightFormat(_height);
            return this;
        }
        public FamilyInstanceBuilder WithHeight(double height)
        {
            _height = height;
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
        public FamilyInstanceBuilder WithHost(Reference host)
        {
            Host = host;
            return this;
        }
        /*public FamilyInstanceBuilder WithOffset(double offset = 0)
        {
            if (offset != 0) _offset = offset;
            return this;
        }*/
        #endregion

        #region Private methods
        private Reference PickHost() => 
            _selection.PickObject(ObjectType.PointOnElement, "Pick a host in the model");
        private ElementId FamilyInstanceCreate(FamilySymbol symbol)
        {
            FamilyInstance newFamilyInstance = null;
            using (var tr = new Transaction(_document, "Creating new FamilyInstance"))
            {
                tr.Start();
                XYZ dir = new XYZ(0, 0, 0);
                /*XYZ location = _offset != 0 
                    ? new XYZ(Host.GlobalPoint.X + _offset, Host.GlobalPoint.Y, Host.GlobalPoint.Z)
                    : Host.GlobalPoint; */

                newFamilyInstance = _uiDocument
                    .Document.Create
                    .NewFamilyInstance(Host, Host.GlobalPoint, dir, symbol);
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
                if (_currentLevelId != null) SetCurrentLevel(familyInstance);
                if (_locationStrategy != null) _locationStrategy.SetRequiredLocation(familyInstance, _height);
                if (string.IsNullOrEmpty(_renderedHeight)) SetHeight(familyInstance);
                if (string.IsNullOrEmpty(_circuit)) SetCircuit(familyInstance);
                /*string category = familyInstance.Category.Name;
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
                }*/
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
                    circuitParam.Set(_renderedHeight);
                    break;
            }
        }
        #endregion
    }
}
