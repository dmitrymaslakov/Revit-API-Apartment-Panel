using ApartmentPanel.Presentation.Services;
using Autodesk.Revit.UI;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApartmentPanel.Presentation.View
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class MainView : UserControl, IDockablePaneProvider
    {
        private Guid m_targetGuid;
        private DockPosition m_position = DockPosition.Bottom;
        private int m_left = 1;
        private int m_right = 1;
        private int m_top = 1;
        private int m_bottom = 1;

        public MainView(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            /*_ = new DockablePaneProviderData();


            data.InitialState = new DockablePaneState
            {
                DockPosition = m_position
            };

            DockablePaneId targetPane;
            if (m_targetGuid == Guid.Empty)
                targetPane = null;
            else targetPane = new DockablePaneId(m_targetGuid);
            if (m_position == DockPosition.Tabbed)
                data.InitialState.TabBehind = targetPane;


            if (m_position == DockPosition.Floating)
            {
                data.InitialState.SetFloatingRectangle(new Autodesk.Revit.DB.Rectangle(m_left, m_top, m_right, m_bottom));
            }*/
        }
        
        public static DockablePaneId PaneId => new DockablePaneId(new Guid("E6EF9DE9-F5F2-454B-8968-4BA2622E5CE5"));

        public static string PaneName => "Apartment panel";

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            //statusBarItem.Content = null;
        }
    }
}
