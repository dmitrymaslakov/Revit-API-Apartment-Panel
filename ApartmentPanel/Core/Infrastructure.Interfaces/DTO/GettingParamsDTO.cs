using ApartmentPanel.Core.Models;
using System;

namespace ApartmentPanel.Core.Infrastructure.Interfaces.DTO
{
    public class GettingParamsDTO
    {
        public string ElementName { get; set; }
        public Action<string> GetInstanceParameters { get; set; }
    }
}
