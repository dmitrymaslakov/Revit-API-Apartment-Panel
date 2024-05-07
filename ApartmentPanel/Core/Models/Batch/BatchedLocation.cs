namespace ApartmentPanel.Core.Models.Batch
{
    public struct BatchedLocation
    {
        public int X { get; set; }
        public int Y { get; set; }

        public BatchedLocation(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
