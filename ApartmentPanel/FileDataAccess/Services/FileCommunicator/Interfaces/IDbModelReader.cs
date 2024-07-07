using ApartmentPanel.FileDataAccess.Models;
using System;

namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces
{
    internal interface IDbModelReader : IDisposable
    {
        FileDbModel Get();
    }
}
