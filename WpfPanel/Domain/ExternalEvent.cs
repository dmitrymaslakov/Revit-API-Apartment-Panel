using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPanel.Domain
{
    public delegate void PanelHandler();

    public class ExternalEvent
    {
        public event PanelHandler ApartmentPanel;
        public RequestHandler Handler { get; set; }

        private void OnApartmentPanel()
        {
            ApartmentPanel += Handler.Execute;
            ApartmentPanel();
        }

        public void Raise()
        {
            OnApartmentPanel();
            ApartmentPanel -= Handler.Execute;
        }

        public static ExternalEvent Create(RequestHandler handler)
        {
            return new ExternalEvent { Handler = handler };
        }
    }
}
