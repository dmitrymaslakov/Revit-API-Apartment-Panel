using ApartmentPanel.UseCases.ApartmentElements.Dto;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using ApartmentPanel.Utility.Mapping;
using AutoMapper;

namespace ApartmentPanel.Presentation.Models
{
    public class ElectricalElement : IMapWith<ElectricalElementDto>
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Category { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ElectricalElementDto, ElectricalElement>();
            profile.CreateMap<ElectricalElement, ElectricalElementDto>();
        }
    }
}