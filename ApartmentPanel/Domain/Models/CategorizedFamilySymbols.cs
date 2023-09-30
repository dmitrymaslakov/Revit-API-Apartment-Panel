﻿using System.Collections.ObjectModel;

namespace ApartmentPanel.Domain.Models
{
    public class CategorizedFamilySymbols
    {
        public string Category { get; set; }
        public ObservableCollection<ApartmentElement> CategorizedElements { get; set; }
    }
}
