using ApartmentPanel.Core.Models.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace ApartmentPanel.Core.Models
{
    /// <summary>
    /// Represents a circuit of the apartment panel
    /// </summary>
    public class Circuit : IEntity
    {
        public Guid Id { get; }

        public Circuit() => Id = Guid.NewGuid();

        public string Number { get; set; }
        public ObservableCollection<IApartmentElement> Elements { get; set; }
    }
}
