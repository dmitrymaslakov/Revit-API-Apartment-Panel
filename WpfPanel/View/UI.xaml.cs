using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfPanel.View
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI : Window
    {
        public UI(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            Button button = sender as Button;
            string currentCategory = button.Tag as string;
            string targetCategory = "Lighting Fixtures";

            if (!currentCategory.Contains(targetCategory))
                return;

            string characterValue = "";
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                char numericChar = (char)('0' + (e.Key - Key.D0));
                characterValue = numericChar.ToString();
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                char numericChar = (char)('0' + (e.Key - Key.NumPad0));
                characterValue = numericChar.ToString();
            }
            currentSuffix.Command?.Execute(characterValue);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button.Tag.ToString() == "Lighting Fixtures")
                statusBarItem.Content = "You should select the lamp(s) before inserting the switch";
            button.Focus();
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            statusBarItem.Content = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestImage.Source = Clipboard.GetImage();
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, "Resources", "Annotations", "Single Socket.png");

            var bs = BitmapFromUri(new Uri(fileName));

            IsFileInUse(fileName);

            TestImage.Source = bs;
            bs = null;
            TestImage.Source = null;
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(Clipboard.GetImage()));
                encoder.Save(fileStream);
            }
        }

        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        static bool IsFileInUse(string filePath)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // The file is not in use by another process
                    return false;
                }
            }
            catch (IOException)
            {
                // The file is in use by another process
                return true;
            }
        }
    }
}
