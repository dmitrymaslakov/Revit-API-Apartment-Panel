using ApartmentPanel.Utility;
using System;
using System.Windows;

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for EditPanel.xaml
    /// </summary>
    public partial class ConfigPanel : Window
    {
        public ConfigPanel(object dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
            OkBtn.CommandParameter = new Action(Confige.Close);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // Initialize the clipboard now that we have a window source to use
            var windowClipboardManager = new ClipboardManager(this);
            windowClipboardManager.ClipboardChanged += ClipboardChanged;
        }

        private void ClipboardChanged(object sender, EventArgs e)
        {
            mainAnnotationPreview.SetAnnotationPreviewCommand?.Execute(Clipboard.GetImage());
        }
    }
}
