﻿using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPanel.Domain;

namespace WpfPanel.ViewModel
{
    public class UIViewModel : ViewModelBase
    {
        private string _message;
        //private readonly UIApplication _uiapp;
        public Request Request { get; private set; }

        public UIViewModel(UIApplication uiapp)
        {
            //_uiapp = uiapp;
            Greeting = new GreetingCommand(this);
            PlacementFixturies = new PlacementFixturiesCommand(this);
            Request = new Request();
        }

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }
        public ICommand Greeting 
        { 
            get; 
            set; 
        }
        public ICommand PlacementFixturies
        {
            get;
            set;
        }

    }
}
