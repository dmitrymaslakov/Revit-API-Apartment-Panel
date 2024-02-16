using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ApartmentPanel.Utility.Exceptions;
using ApartmentPanel.Utility;
using ApartmentPanel.Infrastructure.Models.LocationStrategies;
//using ApartmentPanel.Core.Models;
using System;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;

namespace ApartmentPanel.Infrastructure.Models
{
    public class FamilyInstanceBuilder : RevitInfrastructureBase
    {
        #region Private fields
        private double _height;
        private string _renderedHeight;
        private ILocationStrategy _locationStrategy;
        private ElementId _currentLevelId;
        private string _circuit;
        private string _lampSuffix;
        private string _switchNumbers;
        private Dictionary<string, string> _parameters;
        private string _responsibleForHeightParameter;
        private string _responsibleForCircuitParameter;

        #endregion

        public FamilyInstanceBuilder(UIApplication uiapp) : base(uiapp) { }

        public Reference Host { get; private set; }
        //public BuiltInstance BuiltInstance { get; private set; }

        public BuiltInstance Build(string elementName)
        {
            var familySymbol = new FilteredElementCollector(_document)
                .OfClass(typeof(FamilySymbol))
                .Where(fs => fs.Name == elementName)
                .FirstOrDefault() as FamilySymbol;

            if (Host == null)
                Host = PickHost();

            ElementId familyInstanceId = FamilyInstanceCreate(familySymbol);
            BuiltInstance builtInstance = new BuiltInstance(_uiapp, familyInstanceId);
            FamilyInstanceConfigure(builtInstance);
            return builtInstance;
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
        public FamilyInstanceBuilder WithResponsibleForHeight(string parameterName)
        {
            _responsibleForHeightParameter = parameterName;
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
        public FamilyInstanceBuilder WithResponsibleForCircuit(string parameterName)
        {
            _responsibleForCircuitParameter = parameterName;
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
        public FamilyInstanceBuilder WithParameters(Dictionary<string, string> parameters)
        {
            _parameters = parameters;
            return this;
        }
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

                newFamilyInstance = /*_uiDocument
                    .Document*/
                    _document
                    .Create
                    .NewFamilyInstance(Host, Host.GlobalPoint, dir, symbol);
                tr.Commit();
            }
            return newFamilyInstance.Id;
        }
        private void FamilyInstanceConfigure(BuiltInstance builtInstance)
        {
            FamilyInstance familyInstance = _document.GetElement(builtInstance.Id) as FamilyInstance;

            if (_currentLevelId != null) SetCurrentLevel(familyInstance);
            using (var tr = new Transaction(_document, "Config FamilyInstance"))
            {
                tr.Start();
                if (!string.IsNullOrEmpty(_renderedHeight)) SetHeight(familyInstance);
                if (!string.IsNullOrEmpty(_circuit)) SetCircuit(familyInstance);
                if (_parameters != null) SetParameters(familyInstance);
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
            if (_locationStrategy != null) _locationStrategy.SetRequiredLocation(builtInstance, _height);
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
            using (var tr = new Transaction(_document, "Set current level"))
            {
                tr.Start();
                familyInstance
                .get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM)
                .Set(_currentLevelId);
                tr.Commit();
            }
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
            //string customParameter = StaticData.ELEMENT_CIRCUIT_PARAM_NAME;
            Parameter circuitParam = familyInstance.LookupParameter(_responsibleForCircuitParameter);
            if (circuitParam == null)
                return;
            //throw new CustomParameterException(customParameter, familyInstance.Name);

            string category = familyInstance.Category.Name;
            switch (category)
            {
                case StaticData.LIGHTING_FIXTURES:
                    _lampSuffix = string.IsNullOrEmpty(_lampSuffix) ? _lampSuffix : "/" + _lampSuffix;
                    circuitParam.Set(_circuit + _lampSuffix);
                    break;
                case StaticData.LIGHTING_DEVICES:
                    _switchNumbers = string.IsNullOrEmpty(_switchNumbers) ? _switchNumbers : "/" + _switchNumbers;
                    circuitParam.Set(_circuit + _switchNumbers);
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
            //string customParameter = StaticData.ELEMENT_HEIGHT_PARAM_NAME;
            Parameter circuitParam = familyInstance.LookupParameter(_responsibleForHeightParameter);
            if (circuitParam == null)
                return;
            //throw new CustomParameterException(customParameter, familyInstance.Name);

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
        private void SetParameters(FamilyInstance familyInstance)
        {
            foreach (var parameter in _parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Value))
                {
                    Parameter instanceParam = familyInstance.LookupParameter(parameter.Key);
                    if (instanceParam == null)
                        return;
                    //throw new CustomParameterException(parameter.Key, familyInstance.Name);

                    if (double.TryParse(parameter.Value, out double valueAsDouble))
                    {
                        double valueAsDoubleInFeet = UnitUtils.ConvertToInternalUnits(valueAsDouble,
                            _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
                        instanceParam.Set(valueAsDoubleInFeet);
                    }
                    else
                    {
                        instanceParam.Set(parameter.Value);
                    }
                }
            }
        }
        #endregion
    }
}
