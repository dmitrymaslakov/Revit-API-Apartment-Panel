using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WpfPanel.Domain.Services.Commands;
using WpfPanel.ViewModel;

namespace WpfPanel.Domain
{
    public class PlacementFixturiesCommand : BaseCommand
    {
        public UIViewModel UIViewModel { get; set; }

        public PlacementFixturiesCommand(UIViewModel uIViewModel)
        {
            UIViewModel = uIViewModel;
        }

        public override void Execute(object parameter)
        {
            //MakeRequest(RequestId.Insert);
        }

        private void MakeRequest(RequestId request)
        {
        }
    }
}
