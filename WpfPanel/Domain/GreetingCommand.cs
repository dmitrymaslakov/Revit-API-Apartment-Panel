using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPanel.ViewModel;

namespace WpfPanel.Domain
{
    public class GreetingCommand : BaseCommand
    {
        public UIViewModel UIViewModel { get; set; }

        public GreetingCommand(UIViewModel uIViewModel)
        {
            UIViewModel = uIViewModel;
        }

        public override void Execute(object parameter)
        {
            UIViewModel.Message = "Hello, MVVM";
        }
    }
}
