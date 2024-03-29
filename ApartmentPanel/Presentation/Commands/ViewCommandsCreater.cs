﻿using ApartmentPanel.Core.Services.Interfaces;
using System;
using System.Windows.Input;
using System.Windows;
using ApartmentPanel.Presentation.ViewModel.Interfaces;
using ApartmentPanel.Presentation.View.Components;
using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Core.Enums;
using ApartmentPanel.Core.Models;
using ApartmentPanel.Presentation.Models.Batch;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.Services;

namespace ApartmentPanel.Presentation.Commands
{
    public class ViewCommandsCreater : BaseCommandsCreater
    {
        private readonly IMainViewModel _viewProperties;

        public ViewCommandsCreater(IMainViewModel viewProperties,
            IElementService apartmentElementService) : base(apartmentElementService) =>
            _viewProperties = viewProperties;

        public ICommand CreateConfigureCommand() => new RelayCommand(o =>
        {
            //new ConfigPanel(_viewProperties.ConfigPanelVM).Show();
            ConfigPanelWindowService.OpenWindow(_viewProperties.ConfigPanelVM);
        });

        public ICommand CreateInsertElementCommand() => new RelayCommand(o =>
        {
            /*(string circuit, string elementName, string elementCategory, ImageSource annotation) =
                (ValueTuple<string, string, string, ImageSource>)o;*/
            (string circuit, IApartmentElement element) = (ValueTuple<string, IApartmentElement>)o;

            string insertingMode = "single";

            if (Keyboard.Modifiers == ModifierKeys.Control)
                insertingMode = "multiple";

            element.MountingHeight.FromLevel = element.MountingHeight.FromFloor + _viewProperties.FloorHeight;
            element.MountingHeight.ResponsibleForHeightParameter = _viewProperties.ConfigPanelVM.ResponsibleForHeight;

            var elementDTO = new InsertElementDTO()
            {
                Name = element.Name,
                Category = element.Category,
                Circuit = new CircuitDTO
                {
                    Number = circuit,
                    ResponsibleForCircuitParameter = _viewProperties.ConfigPanelVM.ResponsibleForCircuit
                },
                Height = element.MountingHeight.Clone(),
                CurrentSuffix = _viewProperties.CurrentSuffix,
                Parameters = element.Parameters
                        .GroupBy(p => p.Name)
                        .ToDictionary(g => g.Key, g => g.First().Value),
                InsertingMode = insertingMode
            };
            if (_viewProperties.ElementHeight != null)
            {
                elementDTO.Height.FromFloor = _viewProperties.ElementHeight.FromFloor;
                elementDTO.Height.FromLevel = elementDTO.Height.FromFloor + _viewProperties.FloorHeight;
                elementDTO.Height.TypeOf = _viewProperties.ElementHeight.TypeOf;
                elementDTO.Height.ResponsibleForHeightParameter = 
                    _viewProperties.ConfigPanelVM.ResponsibleForHeight;
            }
            _elementService.InsertToModel(elementDTO);
        });

        public ICommand CreateInsertBatchCommand() => new RelayCommand(o =>
        {
            var elementBatch = o as ElementBatch;
            var elementsDTO = new List<InsertElementDTO>();
            foreach (BatchedRow row in elementBatch.BatchedRows)
            {
                foreach (BatchedElement element in row.RowElements)
                {
                    row.MountingHeight.FromLevel = row.MountingHeight.FromFloor + _viewProperties.FloorHeight;
                    row.MountingHeight.ResponsibleForHeightParameter = _viewProperties.ConfigPanelVM.ResponsibleForHeight;
                    
                    InsertElementDTO elDto = new InsertElementDTO
                    {
                        Category = element.Category,
                        Circuit = new CircuitDTO
                        {
                            Number = element.Circuit,
                            ResponsibleForCircuitParameter = _viewProperties.ConfigPanelVM.ResponsibleForCircuit
                        },
                        /*Height = new Height
                        {
                            TypeOf = row.MountingHeight.TypeOf,
                            FromFloor = row.MountingHeight.FromFloor,
                            ResponsibleForHeightParameter = _viewProperties.ConfigPanelVM.ResponsibleForHeight
                        },*/
                        Height = row.MountingHeight.Clone(),
                        Location = new BatchedLocation
                        {
                            X = elementBatch.BatchedRows.IndexOf(row),
                            Y = row.RowElements.IndexOf(element)
                        },
                        Margin = new Thickness
                        {
                            Left = element.Margin.Left,
                            Top = element.Margin.Top,
                            Right = element.Margin.Right,
                            Bottom = element.Margin.Bottom
                        },
                        Name = element.Name,
                        Parameters = element.Parameters
                        .GroupBy(p => p.Name)
                        .ToDictionary(g => g.Key, g => g.First().Value)
                    };
                    elementsDTO.Add(elDto);
                }
            }
            InsertBatchDTO batchDTO = new InsertBatchDTO
            {
                BatchedElements = elementsDTO
            };

            _elementService.InsertBatchToModel(batchDTO);
        });

        public ICommand CreateSetCurrentSuffixCommand()
            => new RelayCommand(o => _viewProperties.CurrentSuffix = o as string);

        public ICommand CreateSetHeightCommand() => new RelayCommand(o =>
        {
            (TypeOfHeight typeOfHeight, double height) = (ValueTuple<TypeOfHeight, double>)o;
            _viewProperties.ElementHeight = new Height { TypeOf = typeOfHeight, FromFloor = height };
            if (_viewProperties.IsResetHeight) _viewProperties.IsResetHeight = false;
        });

        public ICommand CreateSetStatusCommand() => new RelayCommand(o =>
        {
            switch (o as string)
            {
                case "MouseEnter":
                    _viewProperties.Status = "You should select the lamp(s) before inserting the switch";
                    break;
                case "MouseLeave":
                    _viewProperties.Status = null;
                    break;
            }
        });

        public ICommand CreateResetHeightCommand() => new RelayCommand(o =>
        {
            _viewProperties.IsResetHeight = true;
            _viewProperties.ElementHeight = null;
        });
    }
}
