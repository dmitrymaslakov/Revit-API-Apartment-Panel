using ApartmentPanel.Presentation.Models.Batch;
using System.Windows;
using System.Collections.Generic;

namespace ApartmentPanel.Core.Infrastructure.Interfaces.DTO
{
    public class InsertBatchedElementDTO
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Circuit { get; set; }
        public BatchedLocation Location { get; set; }
        public Thickness Margin { get; set; }
        public Dictionary<string, string> Parameters;
    }
}
