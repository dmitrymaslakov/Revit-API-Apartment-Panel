using ApartmentPanel.Presentation.View.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApartmentPanel.Presentation.Services
{
    public static class ConfigPanelWindowService
    {
        private const int MaxNumberOfOpenWindows = 1;
        private static int _currentNumberOfOpenWindows = 0;
        private static ConfigPanel _configPanel;

        public static void OpenWindow(object dataContext)
        {
            if (_currentNumberOfOpenWindows != MaxNumberOfOpenWindows)
            {
                _configPanel = new ConfigPanel(dataContext);
                _configPanel.Closed += Window_Closed;
                _configPanel.Show();
                _currentNumberOfOpenWindows++;
            }
            else
            {
                _configPanel.Topmost = true;
                _configPanel.Topmost = false;
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            /*ConfigPanel configPanel = (ConfigPanel)sender;
            configPanel.Closed -= Window_Closed;*/
            _configPanel.Closed -= Window_Closed;
            _currentNumberOfOpenWindows--;
        }
    }
}
