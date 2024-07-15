using ApartmentPanel.FileDataAccess.Models;

namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces
{
    internal interface IDbModelService
    {
        FileDbModel Get();
        bool Save(FileDbModel fileDbModel);
    }
}