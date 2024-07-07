using ApartmentPanel.FileDataAccess.Models;
using System.IO;
using System.Text.Json;

namespace ApartmentPanel.FileDataAccess.Services
{
    internal class FileDbService
    {
        private readonly string _fileDbName;

        public FileDbService(string fileDbName)
        {
            _fileDbName = fileDbName;
            string json = File.ReadAllText(_fileDbName);
            FileDbContext = JsonSerializer.Deserialize<FileDbContext>(json);
        }

        internal FileDbContext FileDbContext { get; }
    }
}
