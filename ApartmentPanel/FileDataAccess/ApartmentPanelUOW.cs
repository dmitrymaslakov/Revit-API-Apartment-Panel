using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.FileDataAccess.Models;
using ApartmentPanel.FileDataAccess.Repositories;
using ApartmentPanel.FileDataAccess.Services;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;

namespace ApartmentPanel.FileDataAccess
{
    internal class ApartmentPanelUOW : IUnitOfWork
    {
        private readonly FileDbContext _fileDbContext;
        private readonly FileDbContextFactory _contextFactory;

        public ApartmentPanelUOW(FileDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _fileDbContext = _contextFactory.CreateDbContext(null);
            ApartmentElementRepository = 
                new ApartmentElementRepository(_fileDbContext.Set<ApartmentElement>());
            ElementBatchRepository = new ElementBatchRepository(_fileDbContext.Set<ElementBatch>());
            CircuitRepository = new CircuitRepository(_fileDbContext.Set<Circuit>());
            HeightRepository = new HeightRepository(_fileDbContext.Set<Height>());
            ResponsibleForCircuitRepository =
                new ResponsibleForCircuitRepository(_fileDbContext.ResponsibleForCircuits);
            ResponsibleForHeightRepository =
                new ResponsibleForHeightRepository(_fileDbContext.ResponsibleForHeights);
        }

        public IRepository<ApartmentElement> ApartmentElementRepository { get; }
        public IRepository<ElementBatch> ElementBatchRepository { get; }
        public IRepository<Circuit> CircuitRepository { get; }
        public IRepository<Height> HeightRepository { get; }
        public IRepository<string> ResponsibleForCircuitRepository { get; }
        public IRepository<string> ResponsibleForHeightRepository { get; }
        public string UsedFileDb { get; private set; }

        public void UseFileDb(string databaseName)
        {
            _contextFactory.UpdateContextUsingNewFileDb(_fileDbContext, databaseName);            
            UsedFileDb = databaseName;
        }
        public bool CreateDatabase(string databaseName) => _fileDbContext.CreateDatabase(databaseName);
        public void SaveChanges() => _fileDbContext.SaveChanges();

        public IDbContext GetDbContext() => _fileDbContext.GetDbContext();
    }
}
