using ApartmentPanel.Core.Models;
using ApartmentPanel.Utility.Mapping;
using AutoMapper;

namespace ApartmentPanel.UseCases.ApartmentElements.Dto
{
    public class ApartmentElementCreateDto : IMapWith<ApartmentElement>
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Category { get; set; }
        public string Config { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApartmentElementCreateDto, ApartmentElement>();
        }
    }
}