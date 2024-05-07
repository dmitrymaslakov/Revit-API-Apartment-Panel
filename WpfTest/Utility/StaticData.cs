namespace WpfTest.Utility
{
    public static class StaticData
    {
        public const string TRISSA_SWITCH = "Trissa Switch";
        public const string USB = "USB";
        public const string BLOCK1 = "BLOCK1";
        public const string SINGLE_SOCKET = "Single Socket";
        public const string THROUGH_SWITCH = "Through Switch";
        public const string LAMP = "Lamp";
        public const string DEFAULT_FAMILY = "Default Family";

        public const string TELEPHONE_DEVICES = "Telephone Devices";
        public const string COMMUNICATION_DEVICES = "Communication Devices";
        public const string FIRE_ALARM_DEVICES = "Fire Alarm Devices";
        public const string LIGHTING_DEVICES = "Lighting Devices";
        public const string LIGHTING_FIXTURES = "Lighting Fixtures";
        public const string ELECTRICAL_FIXTURES = "Electrical Fixtures";
        public const string DEFAULT_CATEGORY = "Default Category";


        public const string ELEMENT_HEIGHT_PARAM_NAME = "H-UK";//"UK-HEIGHT";"ASRV-H"
        public const string ELEMENT_CIRCUIT_PARAM_NAME = "NA-CIRCUIT";//"RBX-CIRCUIT" "ASRV Circuit Number"

        public static string GetFamily(int i)
        {
            if (i >= 0 && i < 5)
            {
                return TRISSA_SWITCH;
            }
            else if (i >= 5 && i < 10)
            {
                return LAMP;
            }
            else if (i >= 10 && i < 15)
            {
                return USB;
            }
            else
            {
                return DEFAULT_FAMILY;
            }
        }
        public static string GetCategory(int i)
        {
            if (i >= 0 && i < 5)
            {
                return ELECTRICAL_FIXTURES;
            }
            else if (i >= 5 && i < 10)
            {
                return LIGHTING_FIXTURES;
            }
            else if (i >= 10 && i < 15)
            {
                return COMMUNICATION_DEVICES;
            }
            else
            {
                return DEFAULT_CATEGORY;
            }
        }

    }
}
