using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Infrastructure.Models.LocationStrategies;
using ApartmentPanel.Core.Enums;

namespace ApartmentPanel.Infrastructure.Models
{
    public class ElementInstaller
    {
        private readonly InsertElementDTO _elementData;
        private readonly FamilyInstanceBuilder _familyInstanceBuilder;
        private readonly ILocationStrategy _locationStrategy;

        public ElementInstaller(InsertElementDTO elementData, 
            FamilyInstanceBuilder familyInstanceBuilder)
        {
            _elementData = elementData;
            _familyInstanceBuilder = familyInstanceBuilder;
            _locationStrategy = GetLocationStrategy(_elementData.Height.TypeOf);
        }
        
        public ElementInstaller(string[] elements, FamilyInstanceBuilder familyInstanceBuilder)
        {
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
                .WithHeight(_elementData.Height, _locationStrategy)
                .WithCircuit(_elementData.Circuit)
                .WithCurrentLevel()
                .Build(_elementData.Name);
        }

        public void InstallElectricalFixtures()
        {
            _familyInstanceBuilder
                .WithHeight(_elementData.Height, _locationStrategy)
                .WithCircuit(_elementData.Circuit)
                .WithCurrentLevel()
                .Build(_elementData.Name);
        }

        public void InstallCommunicationDevices()
        {
            _familyInstanceBuilder
                .WithHeight(_elementData.Height, _locationStrategy)
                .WithCurrentLevel()
                .Build(_elementData.Name);
        }

        private ILocationStrategy GetLocationStrategy(TypeOfHeight typeOfHeight)
        {
            switch (typeOfHeight) 
            {
                case TypeOfHeight.UK:
                    return new BottomCenterLocationStrategy();
                case TypeOfHeight.OK:
                    return new TopCenterLocationStrategy();
                case TypeOfHeight.Center:
                    break;
            }
            return null;
        }
    }
}
