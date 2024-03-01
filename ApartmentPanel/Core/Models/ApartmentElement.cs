﻿using ApartmentPanel.Core.Models.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ApartmentPanel.Core.Models
{
    public class ApartmentElement : BaseElement, IApartmentElement
    {
        public Height MountingHeight { get; set; }
        public IApartmentElement Clone()
        {
            ObservableCollection<Parameter> newP = Parameters?.ToList() == null
                ? new ObservableCollection<Parameter>()
                : new ObservableCollection<Parameter>(Parameters?.ToList());

            return new ApartmentElement
            {
                Name = Name,
                Category = Category,
                Annotation = Annotation?.Clone(),
                MountingHeight = MountingHeight?.Clone(),
                Parameters = newP
            };
        }
    }
}
