using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;
using System.Collections.Generic;

namespace ApartmentPanel.Infrastructure.Interfaces.DataAccess
{
    public interface IDbContext
    {
        ICollection<ApartmentElement> ApartmentElements { get; set; }
        ICollection<ElementBatch> ElementBatches { get; set; }
        ICollection<Circuit> Circuits { get; set; }
        ICollection<Height> Heights { get; set; }
        ICollection<string> ResponsibleForHeights { get; set; }
        ICollection<string> ResponsibleForCircuits { get; set; }
    }
}
