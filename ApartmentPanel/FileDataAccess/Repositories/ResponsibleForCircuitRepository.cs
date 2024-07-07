using System.Collections.Generic;

namespace ApartmentPanel.FileDataAccess.Repositories
{
    internal class ResponsibleForCircuitRepository : BaseRepository<string>
    {
        public ResponsibleForCircuitRepository(ICollection<string> entityCollection)
            : base(entityCollection) { }
    }
}
