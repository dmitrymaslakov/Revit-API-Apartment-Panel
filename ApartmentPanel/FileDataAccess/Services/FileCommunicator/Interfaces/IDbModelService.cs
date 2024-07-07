using ApartmentPanel.FileDataAccess.Models;

namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces
{
    internal interface IDbModelService
    {
        //bool IsDbModelExists();
        FileDbModel Get();
        FileDbModel Save(FileDbModel fileDbModel);
    }
}