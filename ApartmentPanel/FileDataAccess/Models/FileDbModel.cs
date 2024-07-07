using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using System.Collections.Generic;

namespace ApartmentPanel.FileDataAccess.Models
{
    internal class FileDbModel : IDbContext
    {
        public ICollection<ApartmentElement> ApartmentElements { get; set; }
        public ICollection<ElementBatch> ElementBatches { get; set; }
        public ICollection<Circuit> Circuits { get; set; }
        public ICollection<Height> Heights { get; set; }
        public ICollection<string> ResponsibleForHeights { get; set; }
        public ICollection<string> ResponsibleForCircuits { get; set; }
    }
}
