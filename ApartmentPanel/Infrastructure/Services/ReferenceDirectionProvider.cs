using ApartmentPanel.Core.Enums;
using Autodesk.Revit.DB;

namespace ApartmentPanel.Infrastructure.Services
{
    internal class ReferenceDirectionProvider
    {
        private readonly FamilySymbol _symbol;

        public ReferenceDirectionProvider(Direction direction, FamilySymbol symbol)
        {
            Direction = direction;
            _symbol = symbol;
        }

        internal Direction Direction { get; set; }

        internal XYZ GetReferenceDirection()
        {
            switch (Direction)
            {
                case Direction.West:
                    return new XYZ(0, -1, 0);
                case Direction.North:
                    return new XYZ(-1, 0, 0);
                case Direction.East:
                    return new XYZ(0, 1, 0);
                case Direction.South:
                    return new XYZ(1, 0, 0);
                case Direction.None:
                default: 
                    return new XYZ(0, 0, 0);
            }            
        }
        private double GetAngleBetweenBasisXAxisAndCurrentXAxis()
        {
            Transform identity = Transform.Identity;
            XYZ localXAxis = identity.BasisX;
            XYZ globalXAxis = XYZ.BasisX;
            return globalXAxis.AngleOnPlaneTo(localXAxis, XYZ.BasisX);
        }
    }
}
