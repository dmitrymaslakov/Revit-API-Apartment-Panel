﻿using DependencyInjectionTest.Core.Models.Interfaces;
using DependencyInjectionTest.Presentation.ViewModel.ComponentsVM;
using DependencyInjectionTest.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DependencyInjectionTest.Presentation.ViewModel.Interfaces
{
    public interface IConfigPanelViewModel
    {
        BitmapSource AnnotationPreview { get; set; }
        bool IsCancelEnabled { get; set; }
        string LatestConfigPath { get; }
        string NewCircuit { get; set; }
        Action<object, OkApplyCancel> OkApplyCancelActions { get; set; }
        ObservableCollection<IApartmentElement> ApartmentElements { get; set; }
        ObservableCollection<IApartmentElement> CircuitElements { get; set; }
        ObservableDictionary<string, ObservableCollection<IApartmentElement>> PanelCircuits { get; set; }
        ObservableCollection<IApartmentElement> SelectedApartmentElements { get; set; }
        ObservableCollection<IApartmentElement> SelectedCircuitElements { get; set; }
        ObservableCollection<KeyValuePair<string, ObservableCollection<IApartmentElement>>> SelectedPanelCircuits { get; set; }
        ICommand AddApartmentElementCommand { get; set; }
        ICommand AddElementToCircuitCommand { get; set; }
        ICommand AddPanelCircuitCommand { get; set; }
        ICommand ApplyCommand { get; set; }
        ICommand CancelCommand { get; set; }
        ICommand LoadLatestConfigCommand { get; set; }
        ICommand OkCommand { get; set; }
        ICommand RemoveApartmentElementsCommand { get; set; }
        ICommand RemoveElementsFromCircuitCommand { get; set; }
        ICommand RemovePanelCircuitsCommand { get; set; }
        ICommand SaveLatestConfigCommand { get; set; }
        ICommand SelectedApartmentElementsCommand { get; set; }
        ICommand SelectedCircuitElementCommand { get; set; }
        ICommand SelectedPanelCircuitCommand { get; set; }
        ICommand SetAnnotationPreviewCommand { get; set; }
        ICommand SetAnnotationToElementCommand { get; set; }

        ConfigPanelViewModel ApplyLatestConfiguration(ConfigPanelViewModel latestConfiguration);
    }
}