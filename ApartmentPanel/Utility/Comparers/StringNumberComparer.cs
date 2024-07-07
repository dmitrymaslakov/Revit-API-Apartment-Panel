using System.Collections.Generic;

namespace ApartmentPanel.Utility.Comparers
{
    public class StringNumberComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (int.TryParse(x, out int xInt) && int.TryParse(y, out int yInt))
                return xInt.CompareTo(yInt);

            return x.CompareTo(y);
        }
    }
}
