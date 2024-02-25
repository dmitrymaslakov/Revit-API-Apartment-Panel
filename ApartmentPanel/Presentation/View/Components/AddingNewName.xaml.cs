using System;
using System.Collections.Generic;
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

namespace ApartmentPanel.Presentation.View.Components
{
    /// <summary>
    /// Interaction logic for AddingNewName.xaml
    /// </summary>
    public partial class AddingNewName : UserControl
    {
        #region HeaderProperty
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(AddingNewName),
                new PropertyMetadata(string.Empty));
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion
        
        #region NewNameProperty
        public static readonly DependencyProperty NewNameProperty =
            DependencyProperty.Register(nameof(NewName), typeof(string), typeof(AddingNewName),
                new PropertyMetadata(string.Empty));
        public string NewName
        {
            get { return (string)GetValue(NewNameProperty); }
            set { SetValue(NewNameProperty, value); }
        }
        #endregion

        #region GetNewNameCommandProperty
        public static readonly DependencyProperty GetNewNameCommandProperty =
            DependencyProperty.Register(nameof(GetNewNameCommand), typeof(ICommand),
                typeof(AddingNewName), new PropertyMetadata(null));
        public ICommand GetNewNameCommand
        {
            get { return (ICommand)GetValue(GetNewNameCommandProperty); }
            set { SetValue(GetNewNameCommandProperty, value); }
        }
        #endregion

        public AddingNewName()
        {
            InitializeComponent();
        }
    }
}
