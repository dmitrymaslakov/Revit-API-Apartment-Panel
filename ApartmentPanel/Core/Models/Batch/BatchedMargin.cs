using ApartmentPanel.Presentation.ViewModel;

namespace ApartmentPanel.Core.Models.Batch
{
    public class BatchedMargin : ViewModelBase
    {
        public BatchedMargin() { }
        public BatchedMargin(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        private double _left;
        public double Left
        {
            get => _left;
            set => Set(ref _left, value);
        }

        private double _top;
        public double Top
        {
            get => _top;
            set => Set(ref _top, value);
        }

        private double _right;
        public double Right
        {
            get => _right;
            set => Set(ref _right, value);
        }

        private double _bottom;
        public double Bottom
        {
            get => _bottom;
            set => Set(ref _bottom, value);
        }

    }
}
