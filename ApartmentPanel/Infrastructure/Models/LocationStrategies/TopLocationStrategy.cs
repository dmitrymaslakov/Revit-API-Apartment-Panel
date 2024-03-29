﻿using Autodesk.Revit.UI;
using ApartmentPanel.Infrastructure.Enums;

namespace ApartmentPanel.Infrastructure.Models.LocationStrategies
{
    public class TopLocationStrategy : LocationStrategyBase, ILocationStrategy
    {
        public TopLocationStrategy(UIApplication uiapp) : base(uiapp) { }

        public void SetRequiredLocation(BuiltInstance builtInstance, double height)
        {
            LocationType locationType = LocationType.Top;
            SetRequiredLocationBase(builtInstance, height, locationType);
        }
    }
}
