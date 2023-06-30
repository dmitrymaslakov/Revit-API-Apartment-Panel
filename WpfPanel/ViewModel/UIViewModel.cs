using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPanel.Domain;
using WpfPanel.Domain.Models;
using WpfPanel.Domain.Services.Commands;

namespace WpfPanel.ViewModel
{
    public class UIViewModel : ViewModelBase
    {
        private readonly RequestHandler _handler;

        public UIViewModel(RequestHandler handler)
        {
            _handler = handler;

            Circuits = new ObservableCollection<Circuit>
            {
                new Circuit { Number = 1 },
                new Circuit { Number = 2 },
                new Circuit { Number = 3 },
            };
            Configure = new ConfigureCommand(o => {
                _handler.Request.Make(RequestId.Configure);
            });

        }
        
        private ObservableCollection<Circuit> _circuits;

        public ObservableCollection<Circuit> Circuits
        {
            get => _circuits;
            set => Set(ref _circuits, value);
        }

        public ICommand Configure { get; set; }
    }
}
