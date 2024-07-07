using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;

namespace ApartmentPanel.Infrastructure.Interfaces.DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<ApartmentElement> ApartmentElementRepository { get; }
        IRepository<ElementBatch> ElementBatchRepository { get; }
        IRepository<Circuit> CircuitRepository { get; }
        IRepository<Height> HeightRepository { get; }
        string UsedFileDb { get; }
        IRepository<string> ResponsibleForCircuitRepository { get; }
        IRepository<string> ResponsibleForHeightRepository { get; }

        bool CreateDatabase(string databaseName);
        void UseFileDb(string databaseName);
        IDbContext GetDbContext();
        void SaveChanges();
    }
}
