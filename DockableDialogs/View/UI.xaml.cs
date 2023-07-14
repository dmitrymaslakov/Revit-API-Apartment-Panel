﻿using Autodesk.Revit.UI;
using DockableDialogs.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DockableDialogs.View
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    //public partial class UI : Window, IDockablePaneProvider
    public partial class UI : UserControl, IDockablePaneProvider
    {
        private Guid m_targetGuid;
        private DockPosition m_position = DockPosition.Bottom;
        private int m_left = 1;
        private int m_right = 1;
        private int m_top = 1;
        private int m_bottom = 1;

        public UI(object dataContext)
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
    }
}