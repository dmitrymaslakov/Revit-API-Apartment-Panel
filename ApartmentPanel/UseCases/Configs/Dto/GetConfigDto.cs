using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using ApartmentPanel.Utility.Mapping;
using AutoMapper;
using System.Collections.Generic;

namespace ApartmentPanel.UseCases.Configs.Dto
{
    public class GetConfigDto : IMapWith<IDbContext>
    {
        public ICollection<ApartmentElement> ApartmentElements { get; set; }
        public ICollection<ElementBatch> ElementBatches { get; set; }
        public ICollection<Circuit> Circuits { get; set; }
        public ICollection<Height> Heights { get; set; }
        public ICollection<string> ResponsibleForHeights { get; set; }
        public ICollection<string> ResponsibleForCircuits { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IDbContext, GetConfigDto>();
        }
    }
}