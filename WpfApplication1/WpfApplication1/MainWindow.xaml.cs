using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
using WpfApplication1;
using System.Windows.Controls.DataVisualization.Charting;
namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {


            InitializeComponent();

            GridPanel gp = new GridPanel("1", 4, 4, 0, 0);
           

            //Setting data for column chart
            grid.Children.Add(gp);
            // provera API-ja

            

        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //(sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }
        private void horizontal_split(object sender, RoutedEventArgs e)
        {
            //Button senderr = (Button)sender;
            GridPanel.horizontalSplitClick(sender, e);
        }
        private void vertical_split(object sender, RoutedEventArgs e)
        {
            //Button senderr = (Button)sender;
            MenuItem menuItem = (MenuItem)sender;


            String name = menuItem.GetValue(FrameworkElement.NameProperty) as String;
            String[] splitedName = name.Split('_');
            var grid = (Grid)this.FindName("grid_" + splitedName[1]);
            var mainGrid = (Grid)this.FindName("grid");
            int rowSpan = (int)grid.GetValue(Grid.RowSpanProperty);
            if (rowSpan == 2)
            {
                menuItem.IsEnabled = false;
            }
            if (rowSpan != 1)
            {
                grid.SetValue(Grid.RowSpanProperty, rowSpan / 2);
                GridPanel gridPanel = new GridPanel("2", 4, 8, 0, 4);
                System.Windows.MessageBox.Show("novi grid name = " + gridPanel.Name);
                mainGrid.Children.Add(gridPanel);
            }
        }
        private T FindParent<T>(DependencyObject child)

        where T : DependencyObject

        {

            T parent = VisualTreeHelper.GetParent(child) as T;

            if (parent != null)

                return parent;

            else

                return FindParent<T>(parent);

        }
        private void showColumnChart()
        {

        }



    }
}
