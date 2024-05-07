using ApartmentPanel.Presentation.ViewModel;
using ApartmentPanel.Utility.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ApartmentPanel.Core.Models.Batch
{
    /// <summary>
    /// Describes a row of elements that is a part of a batch.
    /// </summary>
    public class BatchedRow : ViewModelBase, IDeepClone<BatchedRow>
    {
        public BatchedRow()
        {
            RowElements = new ObservableCollection<BatchedElement>();
            MountingHeight = new Height();
            Margins = new List<Thickness>();
        }
        /// <summary>
        /// Number of row starting from top to bottom when looking at the wall.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Represents a height of the element mounting on the vertical face.
        /// </summary>
        public Height MountingHeight { get; set; }
        private ObservableCollection<BatchedElement> _rowElements;
        /// <summary>
        /// A collection of elements in the row
        /// </summary>
        public ObservableCollection<BatchedElement> RowElements
        {
            get => _rowElements;
            set => Set(ref _rowElements, value);
        }
        public List<Thickness> Margins { get; set; }
        public BatchedRow Clone()
        {
            return new BatchedRow
            {
                Number = Number,
                MountingHeight = new Height { TypeOf = MountingHeight.TypeOf, FromFloor = MountingHeight.FromFloor },
                RowElements = new ObservableCollection<BatchedElement>(RowElements.Select(e => e.Clone()))
            };
        }
    }
}
