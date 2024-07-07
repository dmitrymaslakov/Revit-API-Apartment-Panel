using ApartmentPanel.Core.Models;
using ApartmentPanel.Utility.Mapping;
using AutoMapper;

namespace ApartmentPanel.UseCases.ElectricalElements.Dto
{
    public class ElectricalElementDto : IMapWith<ApartmentElement>
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Category { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ElectricalElementDto, ApartmentElement>();
        }
    }
}