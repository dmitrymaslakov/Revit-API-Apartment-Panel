using System.Collections.Generic;
using System.Windows;

namespace WpfTest.Models.Batch
{
    public class BatchedRow
    {
        public int Number { get; set; }
        //public Height HeightFromFloor { get; set; }
        public List<BatchedElement> RowElements { get; set; }
        //public List<Thickness> Margins { get; set; }
    }
}
