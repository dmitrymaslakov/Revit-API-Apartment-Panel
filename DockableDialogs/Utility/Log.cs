using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialogs.Utility
{
    public class Log
    {
        public static void Message(string message, int level = 0)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
            if (level > 0) TaskDialog.Show("Revit", message);
        }
    }
}
