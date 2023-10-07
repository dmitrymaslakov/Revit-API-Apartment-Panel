﻿using ApartmentPanel.Core.Models;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentPanel.Core.Models
{
    public class Circuit
    {
        public string Number { get; set; }
        public ObservableCollection<ApartmentElement> ApartmentElements { get; set; }
    }
}