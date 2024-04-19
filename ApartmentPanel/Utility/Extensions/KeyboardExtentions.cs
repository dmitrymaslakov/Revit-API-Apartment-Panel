using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApartmentPanel.Utility.Extensions
{
    public static class KeyboardExtentions
    {
        public static bool TryGetNumberAsString(this Key key, out string number)
        {
            number = "";
            if (key >= Key.D0 && key <= Key.D9)
            {
                char numericChar = (char)('0' + (key - Key.D0));
                number = numericChar.ToString();
                return true;
            }
            else if (key >= Key.NumPad0 && key <= Key.NumPad9)
            {
                char numericChar = (char)('0' + (key - Key.NumPad0));
                number = numericChar.ToString();
                return true;
            }
            return false;
        }
        public static bool TryGetCharAsString(this Key key, out string charSymbol)
        {
            charSymbol = "";
            if (key >= Key.A && key <= Key.Z)
            {
                charSymbol = $"{key}".ToLower();
                return true;
            }
            return false;
        }
        public static bool TryGetArrow(this Key key, out Key arrow)
        {
            arrow = Key.None;
            if (key >= Key.Left && key <= Key.Down)
            {
                arrow = key;
                return true;
            }
            return false;
        }

    }
}
