using System.Collections.Generic;

namespace WpfTest.MockData
{
    internal class HeightsList
    {
        public static List<double> HeightsOK { get; } = new List<double> { 110, 116 };
        public static List<double> HeightsUK { get; } = new List<double> { 44, 105, 111 };
        public static List<double> HeightsCenter { get; } = new List<double> { 103, 113 };
    }
}
