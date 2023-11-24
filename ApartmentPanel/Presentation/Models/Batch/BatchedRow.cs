using System.Collections.Generic;
using System.Windows.Documents;

namespace ApartmentPanel.Presentation.Models.Batch
{
    public class BatchedRow
    {
        public int Number { get; set; }
        public List<BatchedElement> RowElements { get; set; }
        public Height HeightFromFloor { get; set; }
    }
}
