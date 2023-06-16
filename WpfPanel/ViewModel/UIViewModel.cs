using Autodesk.Revit.UI;
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

        public UIViewModel()
        {
            Greeting = new GreetingCommand(this);
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
    }
}
