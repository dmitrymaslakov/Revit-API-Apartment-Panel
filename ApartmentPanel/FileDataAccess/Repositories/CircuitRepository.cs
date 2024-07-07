using ApartmentPanel.Core.Models;
using System.Collections.Generic;

namespace ApartmentPanel.FileDataAccess.Repositories
{
    internal class CircuitRepository : BaseRepository<Circuit>
    {
        public CircuitRepository(ICollection<Circuit> entityCollection)
            : base(entityCollection) { }
    }
}
