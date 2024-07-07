using ApartmentPanel.FileDataAccess.Models;

namespace ApartmentPanel.FileDataAccess.Services
{
    internal class FileDbProvider
    {
        private readonly string _fileDbName;

        public FileDbProvider(string fileDbName) => _fileDbName = fileDbName;

        public FileDatabase UseFileDatabase() => new FileDatabase(_fileDbName);
    }
}
