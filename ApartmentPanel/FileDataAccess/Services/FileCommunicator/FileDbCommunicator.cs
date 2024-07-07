using ApartmentPanel.FileDataAccess.Models;
using System.IO;
using System.Text.Json;

namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator
{
    internal abstract class FileDbCommunicator
    {
        protected readonly FileDbContext _fileDbModel;
        protected readonly string _fileDbName;

        public FileDbCommunicator(string fileDbName)
        {
            _fileDbName = fileDbName;
            string json = File.ReadAllText(_fileDbName);
            _fileDbModel = JsonSerializer.Deserialize<FileDbContext>(json);
        }
    }
}