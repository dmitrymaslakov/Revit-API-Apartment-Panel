using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPanel.Domain
{
    public static class RequestHandler
    {
        public static void Execute(RequestId reqest)
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
                default:
                    {
                        break;
                    }
            }

            return;
        }

        private static void InsertTriss()
        {
            MessageBox.Show("The triss is inserted");
        }
    }
}
