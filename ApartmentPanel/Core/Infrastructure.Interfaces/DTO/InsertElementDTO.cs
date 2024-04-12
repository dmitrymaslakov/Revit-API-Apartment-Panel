using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.Models.Batch;
using System.Collections.Generic;
using System.Windows;

namespace ApartmentPanel.Core.Infrastructure.Interfaces.DTO
{
    public class InsertElementDTO
    {
        public CircuitDTO Circuit { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string CurrentSuffix { get; set; }
        public Height Height { get; set; }
        public string InsertingMode { get; set; }
        public string SwitchKeys { get; set; }
        public Thickness Margin { get; set; }
        public double Offset { get; set; }
        public BatchedLocation Location { get; set; }
        public Dictionary<string, string> Parameters;
    }
}
