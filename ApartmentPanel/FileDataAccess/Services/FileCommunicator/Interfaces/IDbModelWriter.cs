﻿using ApartmentPanel.FileDataAccess.Models;
using System;

namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces
{
    internal interface IDbModelWriter : IDisposable
    {
        bool Save(FileDbModel fileDbModel);
    }
}
