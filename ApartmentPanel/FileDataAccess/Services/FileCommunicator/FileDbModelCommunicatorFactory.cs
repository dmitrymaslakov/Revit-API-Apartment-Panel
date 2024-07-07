using ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces;

namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator
{
    internal class FileDbModelCommunicatorFactory : IDbModelCommunicatorFactory
    {
        private string _fileDbName;

        public FileDbModelCommunicatorFactory(string fileDbName) => _fileDbName = fileDbName;

        public IDbModelReader CreateDbModelReader()
        {
            return new FileDbModelReader(_fileDbName);
        }
        public IDbModelWriter CreateDbModelWriter()
        {
            return new FileDbModelWriter(_fileDbName);
        }
    }
}
