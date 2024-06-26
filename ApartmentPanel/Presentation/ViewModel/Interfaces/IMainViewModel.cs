﻿using ApartmentPanel.Core.Enums;
using ApartmentPanel.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.ViewModel.Interfaces
{
    public interface IMainViewModel
    {
        ObservableCollection<Circuit> Circuits { get; set; }
        IConfigPanelViewModel ConfigPanelVM { get; set; }
        string CurrentSuffix { get; set; }
        string Status { get; set; }
        ICommand ConfigureCommand { get; set; }
        ICommand InsertElementCommand { get; set; }
        ICommand SetCurrentSuffixCommand { get; set; }
        ObservableCollection<double> ListHeightsOK { get; set; }
        ObservableCollection<double> ListHeightsUK { get; set; }
        ObservableCollection<double> ListHeightsCenter { get; set; }
        ICommand SetHeightCommand { get; set; }
        Height ElementHeight { get; set; }
        ICommand InsertBatchCommand { get; set; }
        ICommand SetStatusCommand { get; set; }
        ICommand ResetHeightCommand { get; set; }
        bool IsResetHeight { get; set; }
        double FloorHeight { get; set; }
        ICommand SetDirectionCommand { get; set; }
        Direction Direction { get; set; }
        ICommand ClearCurrentSuffixCommand { get; set; }
    }
}