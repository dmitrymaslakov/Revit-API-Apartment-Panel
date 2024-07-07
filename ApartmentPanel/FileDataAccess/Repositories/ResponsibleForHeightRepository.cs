using System.Collections.Generic;

namespace ApartmentPanel.FileDataAccess.Repositories
{
    internal class ResponsibleForHeightRepository : BaseRepository<string>
    {
        public ResponsibleForHeightRepository(ICollection<string> entityCollection)
            : base(entityCollection) { }
    }
}
