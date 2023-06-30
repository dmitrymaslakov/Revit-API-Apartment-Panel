using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfPanel.View.Components;

namespace WpfPanel.Domain
{
    public class RequestHandler
    {
        public Request Request { get; } = new Request();

        public void Execute(RequestId reqest)
        {
            switch (reqest)
            {
                case RequestId.None:
                    {
                        return;  // no request at this time -> we can leave immediately
                    }
                case RequestId.Insert:
                    {
                        InsertTriss();
                        break;
                    }
                case RequestId.Configure:
                    {
                        ShowEditPanel();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return;
        }

        private void InsertTriss()
        {
            MessageBox.Show("The triss is inserted");
        }

        private void ShowEditPanel()
        {
            //new EditPanel().ShowDialog();
        }
    }
}
