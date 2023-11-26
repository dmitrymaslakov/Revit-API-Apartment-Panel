using ApartmentPanel.Core.Models;

namespace ApartmentPanel.Core.Infrastructure.Interfaces.DTO
{
    public class InsertElementDTO
    {
        public string Circuit { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string CurrentSuffix { get; set; }
        public Height Height { get; set; }
        public string InsertingMode { get; set; }
        public string SwitchNumbers { get; set; }
    }
}
