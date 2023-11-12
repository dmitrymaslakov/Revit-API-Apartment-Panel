using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfTest.Views.Components
{
    /// <summary>
    /// Interaction logic for HeightsList.xaml
    /// </summary>
    public partial class HeightsList : UserControl
    {
        public HeightsList()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TypeOfHeightProperty =
            DependencyProperty.Register(nameof(TypeOfHeight), typeof(string), typeof(HeightsList),
                new PropertyMetadata(string.Empty));

        public string TypeOfHeight
        {
            get { return (string)GetValue(TypeOfHeightProperty); }
            set { SetValue(TypeOfHeightProperty, value); }
        }

        public static readonly DependencyProperty ListHeightsProperty =
            DependencyProperty.Register(nameof(ListHeights), typeof(ObservableCollection<double>),
                typeof(HeightsList), new PropertyMetadata(null));

        public ObservableCollection<double> ListHeights
        {
            get { return (ObservableCollection<double>)GetValue(ListHeightsProperty); }
            set { SetValue(ListHeightsProperty, value); }
        }


        public static readonly DependencyProperty GetHeightCommandProperty =
            DependencyProperty.Register(nameof(GetHeightCommand), typeof(ICommand),
                typeof(HeightsList), new PropertyMetadata(null));

        public ICommand GetHeightCommand
        {
            get { return (ICommand)GetValue(GetHeightCommandProperty); }
            set { SetValue(GetHeightCommandProperty, value); }
        }

        private void ListHeights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listHeights = sender as ListView;
            string typeOfHeight = ((GridView)listHeights.View).Columns[0].Header as string;
            double height = (double)listHeights.SelectedItem;
            GetHeightCommand?.Execute((typeOfHeight, height));
        }
    }
}
