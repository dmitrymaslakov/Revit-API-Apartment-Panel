﻿using DependencyInjectionTest.Core.Models.Interfaces;
using DependencyInjectionTest.Presentation.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionTest.Presentation.Services
{
    public class CircuitService
    {
        private readonly IConfigPanelViewModel _configPanelViewModel;

        public CircuitService(IConfigPanelViewModel configPanelProperties) => 
            _configPanelViewModel = configPanelProperties;

        public void AddCurrentCircuitElements(ObservableCollection<IApartmentElement> currentCircuitElements)
        {
            if (_configPanelViewModel.CircuitElements.Count != 0)
                _configPanelViewModel.CircuitElements.Clear();

            foreach (var item in currentCircuitElements)
                _configPanelViewModel.CircuitElements.Add(item);
        }
    }
}