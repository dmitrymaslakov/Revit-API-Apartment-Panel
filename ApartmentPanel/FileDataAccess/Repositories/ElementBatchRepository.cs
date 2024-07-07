using ApartmentPanel.Core.Models.Batch;
using System.Collections.Generic;

namespace ApartmentPanel.FileDataAccess.Repositories
{
    internal class ElementBatchRepository : BaseRepository<ElementBatch>
    {
        public ElementBatchRepository(ICollection<ElementBatch> entityCollection)
            : base(entityCollection) { }
    }
}
