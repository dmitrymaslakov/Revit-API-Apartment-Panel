namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces
{
    internal interface IDbModelCommunicatorFactory
    {
        IDbModelReader CreateDbModelReader();
        IDbModelWriter CreateDbModelWriter();
    }
}
