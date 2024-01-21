using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows;

namespace ApartmentPanel.Infrastructure.Models
{
    public class BatchedInstance
    {
        public BuiltInstance Instance { get; set; }
        //public BatchedLocation Location { get; set; }
        public Thickness Margin { get; set; }
        public BatchedInstance Next { get; set; }
        /*public double GetOffset()
        {
            XYZ basePoint = (Instance.Location as LocationPoint)?.Point;
            XYZ maxPoint = Instance.get_BoundingBox(null).Max;
            double fromLocationToInstanceEdge = Math.Abs(basePoint.X - maxPoint.X);
            double leftMarginInFeets = UnitUtils.ConvertToInternalUnits(Margin.Left,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
            return fromLocationToInstanceEdge + leftMarginInFeets;
        }*/
    }
}
