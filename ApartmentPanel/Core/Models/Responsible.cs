using ApartmentPanel.Core.Models.Interfaces;
using System;

namespace ApartmentPanel.Core.Models
{
    public class Responsible : IEntity
    {
        public Responsible() => Id = Guid.NewGuid();
        public Guid Id { get; set; }
    }
}
