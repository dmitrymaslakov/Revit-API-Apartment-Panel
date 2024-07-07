using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.Core.Models.Interfaces;
using ApartmentPanel.Presentation.ViewModel.ComponentsVM;
using ApartmentPanel.Utility.Mapping;
using AutoMapper;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace ApartmentPanel.Presentation.Models
{
    public class LatestConfiguration : IMapWith<ConfigPanelViewModel>
    {
        //public string LatestConfigPath { get; set; }
        public ObservableCollection<IApartmentElement> ApartmentElements { get; set; }
        public ObservableCollection<Circuit> PanelCircuits { get; set; }
        public string ResponsibleForHeight { get; set; }
        public string ResponsibleForCircuit { get; set; }
        //public ElementBatch ElementBatch { get; set; }
        public ObservableCollection<ElementBatch> Batches { get; set; }
        /*public BatchedElement NewElementForBatch { get; set; }
        public BatchedElement SelectedBatchedElement { get; set; }
        public BatchedRow SelectedBatchedRow { get; set; }*/
        public ObservableCollection<double> ListHeightsOK { get; set; }
        public ObservableCollection<double> ListHeightsUK { get; set; }
        public ObservableCollection<double> ListHeightsCenter { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ConfigPanelViewModel, LatestConfiguration>()
                /*.ForMember(latestConf => latestConf.ApartmentElements,
                    opt => opt.MapFrom(vm => vm.ApartmentElementsVM.ApartmentElements))*/
                .ForMember(latestConfAe, opt => opt.MapFrom(configPanelVMAe))
                .ForMember(latestConf => latestConf.PanelCircuits,
                    opt => opt.MapFrom(vm => vm.PanelCircuitsVM.PanelCircuits))
                .ForMember(latestConf => latestConf.ResponsibleForHeight,
                    opt => opt.MapFrom(vm => vm.ResponsibleForHeight))
                .ForMember(latestConf => latestConf.ResponsibleForCircuit,
                    opt => opt.MapFrom(vm => vm.ResponsibleForCircuit))
                .ForMember(latestConf => latestConf.Batches,
                    opt => opt.MapFrom(vm => vm.Batches))
                .ForMember(latestConf => latestConf.ListHeightsOK,
                    opt => opt.MapFrom(vm => vm.ListHeightsOK))
                .ForMember(latestConf => latestConf.ListHeightsUK,
                    opt => opt.MapFrom(vm => vm.ListHeightsUK))
                .ForMember(latestConf => latestConf.ListHeightsCenter,
                    opt => opt.MapFrom(vm => vm.ListHeightsCenter));
        }

        private Expression<Func<LatestConfiguration, ObservableCollection<IApartmentElement>>>
            latestConfAe = latestConf => latestConf.ApartmentElements;
        private Expression<Func<ConfigPanelViewModel, ObservableCollection<IApartmentElement>>>
            configPanelVMAe = vm => vm.ApartmentElementsVM.ApartmentElements;

    }
}
