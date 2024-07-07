using ApartmentPanel.FileDataAccess.Models;
using ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces;

namespace ApartmentPanel.Utility.AnnotationUtility
{
    internal class FileDbModelService : IDbModelService
    {
        private IDbModelCommunicatorFactory _dbModelCommunicatorFactory;

        public FileDbModelService(IDbModelCommunicatorFactory dbModelCommunicatorFactory)
            => SetCommunicatorFactory(dbModelCommunicatorFactory);

        public FileDbModel Get()
        {
            using (IDbModelReader reader = _dbModelCommunicatorFactory.CreateDbModelReader())
            {
                return reader.Get();
            }
        }
        public FileDbModel Save(FileDbModel fileDbModel)
        {
            using (IDbModelWriter writer = _dbModelCommunicatorFactory.CreateDbModelWriter())
            {
                return writer.Save(fileDbModel);
            }
        }
        public void SetCommunicatorFactory(IDbModelCommunicatorFactory dbModelCommunicatorFactory) 
            => _dbModelCommunicatorFactory = dbModelCommunicatorFactory;
        //public bool IsDbModelExists() => _dbModelCommunicatorFactory.IsDbModelExists();
    }
}
