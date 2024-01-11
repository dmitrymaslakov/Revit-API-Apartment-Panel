using ApartmentPanel.Core.Models;
using System;
using System.Collections.Generic;

namespace ApartmentPanel.Core.Infrastructure.Interfaces.DTO
{
    public class SetParamsDTO
    {
        public string ElementName { get; set; }
        public Action<List<string>> SetInstanceParameters { get; set; }
    }
}
