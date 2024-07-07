using ApartmentPanel.Core.Models;
using System.Collections.Generic;

namespace ApartmentPanel.FileDataAccess.Repositories
{
    internal class HeightRepository : BaseRepository<Height>
    {
        public HeightRepository(ICollection<Height> entityCollection)
            : base(entityCollection) { }
    }
}
