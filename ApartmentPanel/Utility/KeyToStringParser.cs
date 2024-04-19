using System.Windows.Input;

namespace ApartmentPanel.Utility
{
    public class KeyToStringParser
    {
        public static string ParseNumber(Key key)
        {
            string characterValue = "";
            if (key >= Key.D0 && key <= Key.D9)
            {
                char numericChar = (char)('0' + (key - Key.D0));
                characterValue = numericChar.ToString();
            }
            else if (key >= Key.NumPad0 && key <= Key.NumPad9)
            {
                char numericChar = (char)('0' + (key - Key.NumPad0));
                characterValue = numericChar.ToString();
            }
            return characterValue;
        }
        public static string ParseChar(Key key)
        {
            string characterValue = "";
            if (key >= Key.A && key <= Key.Z)
            {
                if (char.TryParse(key.ToString(), out char parsedCharacter))
                    characterValue = parsedCharacter.ToString().ToLower();
            }
            return characterValue;
        }
        public static string ParseArrow(Key key)
        {
            string characterValue = "";
            if (key >= Key.Left && key <= Key.Down)
                characterValue = $"{key}";

            return characterValue;
        }
    }
}
