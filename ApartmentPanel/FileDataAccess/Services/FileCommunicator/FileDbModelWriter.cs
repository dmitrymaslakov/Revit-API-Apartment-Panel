using ApartmentPanel.FileDataAccess.Models;
using ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces;
using System;
using System.IO;
using System.Text.Json;

namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator
{
    internal class FileDbModelWriter : IDbModelWriter, IDisposable
    {
        private bool _disposed;
        private readonly string _fullPath;

        public FileDbModelWriter(string fullPath) => _fullPath = fullPath;

        public bool Save(FileDbModel fileDbModel)
        {
            try
            {
                if (_disposed)
                    throw new Exception("Object is disposed.");

                var folderPath = Path.GetDirectoryName(_fullPath);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string json = JsonSerializer.Serialize(fileDbModel);
                File.WriteAllText(_fullPath, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Dispose() => _disposed = true;
    }
}