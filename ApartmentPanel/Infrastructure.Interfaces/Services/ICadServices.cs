using ApartmentPanel.UseCases.ApartmentElements.Dto;
using ApartmentPanel.UseCases.ElectricalElements.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApartmentPanel.Infrastructure.Interfaces.Services
{
    public interface ICadServices
    {
        Task<ICollection<ElectricalElementDto>> GetElectricalElementsAsync();
    }
}