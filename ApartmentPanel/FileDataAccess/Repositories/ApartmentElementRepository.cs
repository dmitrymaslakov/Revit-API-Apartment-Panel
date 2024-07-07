using ApartmentPanel.Core.Models;
using System.Collections.Generic;

namespace ApartmentPanel.FileDataAccess.Repositories
{
    internal class ApartmentElementRepository : BaseRepository<ApartmentElement>
    {
        public ApartmentElementRepository(ICollection<ApartmentElement> entityCollection)
            : base(entityCollection) { }
    }
}
