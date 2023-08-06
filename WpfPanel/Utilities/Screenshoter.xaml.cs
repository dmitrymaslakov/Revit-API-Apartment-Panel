using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WpfPanel.Utilities
{
    /// <summary>
    /// Interaction logic for Screenshoter.xaml
    /// </summary>
    public partial class Screenshoter : Window
    {
        private static Screenshoter _instance;
        private Viewfinder _viewfinder = new Viewfinder();
        #region InteropServices
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();
        #endregion

        private Bitmap ScreenShot = null;
        private System.Windows.Point LocUp;
        private System.Windows.Point LocNow;
        private System.Windows.Point LocPress;
        private bool RectangleFlag = false;

        public Screenshoter()
        {
            InitializeComponent();
            _instance = this;
        }

        public static Bitmap ScreenCapture()
        {
            // Initialize the virtual screen to dummy values
            int screenLeft = int.MaxValue;
            int screenTop = int.MaxValue;
            int screenRight = int.MinValue;
            int screenBottom = int.MinValue;

            Bitmap bmp = null;

            // Enumerate system display devices
            int deviceIndex = 0;
            while (true)
            {
                NativeUtilities.DisplayDevice deviceData = new NativeUtilities.DisplayDevice { cb = Marshal.SizeOf(typeof(NativeUtilities.DisplayDevice)) };
                if (NativeUtilities.EnumDisplayDevices(null, deviceIndex, ref deviceData, 0) != 0)
                {
                    // Get the position and size of this particular display device
                    NativeUtilities.DEVMODE devMode = new NativeUtilities.DEVMODE();
                    if (NativeUtilities.EnumDisplaySettings(deviceData.DeviceName, NativeUtilities.ENUM_CURRENT_SETTINGS, ref devMode))
                    {
                        // Update the virtual screen dimensions
                        screenLeft = Math.Min(screenLeft, devMode.dmPositionX);
                        screenTop = Math.Min(screenTop, devMode.dmPositionY);
                        screenRight = Math.Max(screenRight, devMode.dmPositionX + devMode.dmPelsWidth);
                        screenBottom = Math.Max(screenBottom, devMode.dmPositionY + devMode.dmPelsHeight);
                    }
                    deviceIndex++;
                }
                else
                    break;
            }

            // Create a bitmap of the appropriate size to receive the screen-shot.
            bmp = new Bitmap(screenRight - screenLeft, screenBottom - screenTop);

            Graphics g = Graphics.FromImage(bmp);

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bmp, new Rectangle(System.Drawing.Point.Empty, bmp.Size));
            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);

            return bmp;
        }

        private void m_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LocPress = e.GetPosition(this);
            _viewfinder.Show();
            _viewfinder.Left = LocPress.X;
            _viewfinder.Top = LocPress.Y;
            _viewfinder.RenderSize = new System.Windows.Size(0, 0);
            _viewfinder.Opacity = 0.3;
            RectangleFlag = true;
        }
    }

    class NativeUtilities
    {
        [Flags()]
        public enum DisplayDeviceStateFlags : int
        {
            // The device is part of the desktop.
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            // This is the primary display.
            PrimaryDevice = 0x4,
            // Represents a pseudo device used to mirror application drawing for remoting or other purposes.
            MirroringDriver = 0x8,
            // The device is VGA compatible.
            VGACompatible = 0x16,
            // The device is removable; it cannot be the primary display
            Removable = 0x20,
            // The device has more display modes than its output devices support.
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DisplayDevice
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        public const int ENUM_CURRENT_SETTINGS = -1;
        const int ENUM_REGISTRY_SETTINGS = -2;

        [DllImport("User32.dll")]
        public static extern int EnumDisplayDevices(string lpDevice, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);
    }
}
