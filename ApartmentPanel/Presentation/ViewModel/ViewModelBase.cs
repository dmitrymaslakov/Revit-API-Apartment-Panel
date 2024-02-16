using System.Text.Json.Serialization;
using ApartmentPanel.Core.Services.Interfaces;
using ApartmentPanel.Utility;

namespace ApartmentPanel.Presentation.ViewModel
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;

    public abstract class ViewModelBase : NotifyPropertyChanged
    {
        [JsonIgnore]
        public IElementService ElementService { get; }

        public ViewModelBase() { }

        public ViewModelBase(IElementService elementService) => 
            ElementService = elementService;        
    }
}
