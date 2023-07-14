﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialogs.Domain.Models
{
    public class Circuit
    {
        private const string TRISSA_SWITCH = "Trissa Switch";
        private const string USB = "USB";
        private const string BLOCK1 = "BLOCK1";
        private const string SINGLE_SOCKET = "Single Socket";
        private const string THROUGH_SWITCH = "Through Switch";

        public int Number { get; set; }
        public ObservableCollection<string> ApartmentElements { get; } = new ObservableCollection<string>
        //public List<string> ApartmentElements { get; } = new List<string>
        {
            TRISSA_SWITCH, USB, BLOCK1, SINGLE_SOCKET, THROUGH_SWITCH
        };
    }

    class ApartmentElement
    {
        public string Name { get; set; } 
    }
}