using System.Windows;
using System.Windows.Media;

namespace WpfTest.Models.Batch
{
    public class BatchedElement
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Circuit { get; set; }
        public ImageSource Annotation { get; set; }
        //public BatchedLocation Location { get; set; }
        public Thickness Margin { get; set; }
    }
}
