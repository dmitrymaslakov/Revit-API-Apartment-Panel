using DockableDialogs.Utility;
using System;
using System.Windows;

namespace DockableDialogs.View.Components
{
    /// <summary>
    /// Interaction logic for EditPanel.xaml
    /// </summary>
    public partial class EditPanel : Window
    {
        public EditPanel(object dataContext)
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
            => annotationPreview.Command?.Execute(Clipboard.GetImage());
    }
}
