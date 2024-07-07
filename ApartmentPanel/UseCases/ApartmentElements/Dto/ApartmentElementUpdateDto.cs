using ApartmentPanel.Core.Models;
using ApartmentPanel.Utility.Mapping;
using AutoMapper;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.UseCases.ApartmentElements.Dto
{
    public class ApartmentElementUpdateDto : IMapWith<ApartmentElement>
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Family { get; set; }
        public BitmapImage Annotation { get; set; }
        public Height MountingHeight { get; set; }
        public ICollection<Parameter> Parameters { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApartmentElementCreateDto, ApartmentElement>();
        }
    }
}