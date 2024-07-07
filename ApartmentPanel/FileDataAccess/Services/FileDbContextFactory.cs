using ApartmentPanel.FileDataAccess.Models;

namespace ApartmentPanel.FileDataAccess.Services
{
    internal class FileDbContextFactory
    {
        public FileDbContext CreateDbContext(string fileDbName)
        {
            var provider = new FileDbProvider(fileDbName);
            return new FileDbContext(provider);
        }
        public void UpdateContextUsingNewFileDb(FileDbContext targetContext, string fileDbName)
        {
            var provider = new FileDbProvider(fileDbName);
            targetContext.UpdateContext(provider);
        }
    }
}
