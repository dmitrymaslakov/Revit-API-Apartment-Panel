using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Infrastructure.Models.LocationStrategies;
using ApartmentPanel.Core.Enums;
using System;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Models
{
    public class ElementInstaller
    {
        private readonly InsertElementDTO _elementData;
        private readonly FamilyInstanceBuilder _familyInstanceBuilder;
        private readonly ILocationStrategy _locationStrategy;
        private readonly Func<double, string> _heightFormat;
        private readonly UIApplication _uiapp;

        public ElementInstaller(UIApplication uiapp, InsertElementDTO elementData,
            FamilyInstanceBuilder familyInstanceBuilder)
        {
            _uiapp = uiapp;
            _elementData = elementData;
            _familyInstanceBuilder = familyInstanceBuilder;
            _locationStrategy = GetLocationStrategy(_elementData.Height.TypeOf);
            _heightFormat = height => $"{_elementData.Height.TypeOf}={height}";
        }
        
        public ElementInstaller(UIApplication uiapp, string[] batch, FamilyInstanceBuilder familyInstanceBuilder)
        {
            _uiapp = uiapp;
            _familyInstanceBuilder = familyInstanceBuilder;
        }

        public void InstallLightingFixtures()
        {
            _familyInstanceBuilder
                .WithCircuit(_elementData.Circuit)
                .WithCurrentLevel()
                .WithLampSuffix(_elementData.CurrentSuffix)
                .Build(_elementData.Name);
        }

        public void InstallLightingDevices()
        {
            _familyInstanceBuilder
                .WithSwitchNumbers(_elementData.SwitchNumbers)
                .SetLocationStrategy(_locationStrategy)
                .RenderHeightAs(_heightFormat)
                .WithHeight(_elementData.Height.Value)
                .WithCircuit(_elementData.Circuit)
                .WithCurrentLevel()
                .Build(_elementData.Name);
        }

        public void InstallElectricalFixtures()
        {
            _familyInstanceBuilder
                .SetLocationStrategy(_locationStrategy)
                .RenderHeightAs(_heightFormat)
                .WithHeight(_elementData.Height.Value)
                .WithCircuit(_elementData.Circuit)
                .WithCurrentLevel()
                .Build(_elementData.Name);
        }

        public void InstallCommunicationDevices()
        {
            _familyInstanceBuilder
                .SetLocationStrategy(_locationStrategy)
                .RenderHeightAs(_heightFormat)
                .WithHeight(_elementData.Height.Value)
                .WithCurrentLevel()
                .Build(_elementData.Name);
        }

        private ILocationStrategy GetLocationStrategy(TypeOfHeight typeOfHeight)
        {
            switch (typeOfHeight) 
            {
                case TypeOfHeight.UK:
                    return new BottomLocationStrategy(_uiapp);
                case TypeOfHeight.OK:
                    return new TopLocationStrategy(_uiapp);
                case TypeOfHeight.Center:
                    break;
            }
            return null;
        }
    }
}
