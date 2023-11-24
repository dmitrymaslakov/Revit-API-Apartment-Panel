using ApartmentPanel.Core.Models.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace ApartmentPanel.Presentation.Models.Batch
{
    public class BatchedElement
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Circuit { get; set; }
        public ImageSource Annotation { get; set; }
        public BatchedLocation Location { get; set; }
        public Thickness Margin { get; set; }
    }
}
