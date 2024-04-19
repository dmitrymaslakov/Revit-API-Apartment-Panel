using ApartmentPanel.Presentation.Enums;
using System.Collections.Generic;

namespace ApartmentPanel.Core.Infrastructure.Interfaces.DTO
{
    public class InsertBatchDTO
    {
        //public string Name { get; set; }
        public List<InsertElementDTO> BatchedElements { get; set; }
    }
}
