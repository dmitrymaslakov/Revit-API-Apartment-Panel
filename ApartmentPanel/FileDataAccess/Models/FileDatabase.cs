using ApartmentPanel.FileDataAccess.Services;
using ApartmentPanel.FileDataAccess.Services.FileCommunicator;
using ApartmentPanel.Utility.AnnotationUtility;

namespace ApartmentPanel.FileDataAccess.Models
{
    internal class FileDatabase
    {
        private readonly string _fullDbName;
        private readonly FileDbModelService _dbModelService;

        public FileDatabase(string fileDbName)
        {
            _fullDbName = string.IsNullOrEmpty(fileDbName)
                ? fileDbName : FilePathService.GetFileDbPath(fileDbName);
            var communicatorFactory = new FileDbModelCommunicatorFactory(_fullDbName);
            _dbModelService = new FileDbModelService(communicatorFactory);
        }

        public bool Create(string dbName)
        {
            /*var stringBuilder = new StringBuilder(FileUtility.GetAssemblyPath());
            stringBuilder
            .Append($"\\{dbName}")
            .Append("LatestConfig")
            .Append(".json");*/

            var fileDbModel = new FileDbModel();
            var fullDbName = FilePathService.GetFileDbPath(dbName);
            var communicatorFactory = new FileDbModelCommunicatorFactory(fullDbName);
            //_dbModelService.SetCommunicatorFactory(communicatorFactory);
            var dbModelService = new FileDbModelService(communicatorFactory);
            var dbModel = dbModelService.Save(fileDbModel);
            return dbModel != null;
            /*
            if (!File.Exists(fullDbName))
            {
                string json = JsonSerializer.Serialize(fileDbModel);
                File.WriteAllText(fullDbName, json);
                return true;
            }*/
        }
        public bool SaveChanges(FileDbModel dbModel)
        {
            return _dbModelService.Save(dbModel);
            /*string json = JsonSerializer.Serialize(dbModel);
            File.WriteAllText(_fullDbName, json);*/
        }
        public FileDbModel GetModel() => _dbModelService.Get();
    }
}
