using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Windows.Controls.DataVisualization.Charting;
using System.Timers;
using System.Diagnostics;

namespace WpfApplication1
{
    class GridPanel : Grid
    {
        public delegate void delUpdateGrid();
        ThreadStart threadStart;
        Thread myUpdateThread;
        Chart graphChart;
        public GridPanel(string id, int rowSpan, int columnSpan, int column, int row)
        {
            // set unique id
            this.SetValue(Grid.NameProperty, "grid_" + id);
            this.SetValue(Grid.ColumnSpanProperty, columnSpan);
            this.SetValue(Grid.RowSpanProperty, rowSpan);
            this.SetValue(Grid.ColumnProperty, column);
            this.SetValue(Grid.RowProperty, row);
            Border myBorder1 = new Border();
            myBorder1.BorderBrush = Brushes.Black;
            myBorder1.BorderThickness = new Thickness(1);
            this.Children.Add(myBorder1);

            //border.Child = this;
            initializeComponents();
            //proba grafa


            //proba
            setButton(id, rowSpan, columnSpan);
            id_counter++;
        }
        private void initializeComponents()
        {
            SolidColorBrush redBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("White");
            this.SetValue(Grid.BackgroundProperty, redBrush);


        }
        private void setButton(String id, int rowSpan, int colSpan)
        {
            Button b = new Button();
            b.SetValue(Button.VerticalAlignmentProperty, VerticalAlignment.Top);
            b.SetValue(Button.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            double width = 33;
            double height = 23;
            b.SetValue(Button.WidthProperty, width);
            b.SetValue(Button.HeightProperty, height);
            Path path = new Path();
            path.SetValue(Path.NameProperty, "BtnArrow");
            path.SetValue(Path.VerticalAlignmentProperty, VerticalAlignment.Center);
            //Arrow figure on button

            path.Data = Geometry.Parse("M 301.14,-189.041L 311.57,-189.041L 306.355,-182.942L 301.14, -189.041 Z ");
            ContextMenu buttonMenu = new ContextMenu();
            MenuItem horizontalSplit = new MenuItem();
            horizontalSplit.Name = "horizontal_" + id;
            horizontalSplit.Header = "horizontal split";
            horizontalSplit.Click += horizontalSplitClick;
            if (colSpan == 1)
            {
                horizontalSplit.IsEnabled = false;
            }
            MenuItem verticalSplit = new MenuItem();
            verticalSplit.Name = "vertical_" + id;
            verticalSplit.Header = "vertical split";
            verticalSplit.Click += verticalSplitClick;
            if (rowSpan == 1)
            {
                verticalSplit.IsEnabled = false;
            }
            MenuItem graphDisplay = new MenuItem();
            graphDisplay.Name = "graph_" + id;
            graphDisplay.Header = "graficki prikaz";
            graphDisplay.Click += graphDisplayClick;



            buttonMenu.Items.Add(graphDisplay);
            buttonMenu.Items.Add(horizontalSplit);
            buttonMenu.Items.Add(verticalSplit);
            b.ContextMenu = buttonMenu;
            b.Click += buttonClick;
            b.Content = path;
            this.Children.Add(b);
        }
        public static void buttonClick(object sender, RoutedEventArgs e)
        {
            //(sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }
        public async void graphDisplayClick(object sender, RoutedEventArgs e)
        {


            Task<Task> task = new Task<Task>(refreshGraph);
            task.Start();
            await task;
            

        }
        
        public async Task refreshGraph()
        {
            while (true)
            {
                StockApi api = new WpfApplication1.StockApi();
                Dictionary<string, dynamic> data =  api.getData("?function=TIME_SERIES_INTRADAY&symbol=MSFT&interval=1min&apikey=1ST174M77Q7QPYDW");
                if (data == null)
                {
                    continue;
                }
                //Dictionary<string, dynamic> data = api.getData("/");


                List<KeyValuePair<string, double>> openData = new List<KeyValuePair<string, double>>();
                List<KeyValuePair<string, double>> highData = new List<KeyValuePair<string, double>>();
                List<KeyValuePair<string, double>> lowData = new List<KeyValuePair<string, double>>();
                List<KeyValuePair<string, double>> closeData = new List<KeyValuePair<string, double>>();
                List<KeyValuePair<string, double>> volumeData = new List<KeyValuePair<string, double>>();
                int i = 0;
                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    LineSeries closeLines = new LineSeries();
                    foreach (JProperty timeInterval in data["Time Series (1min)"])
                    {
                        // do something with entry.Value or entry.Key
                        JToken values = timeInterval.First.First;
                        // values.First = 1. open value
                        openData.Add(new KeyValuePair<string, double>(timeInterval.Name, Convert.ToDouble(values.First.ToString())));
                        values = values.Next;
                        highData.Add(new KeyValuePair<string, double>(timeInterval.Name, Convert.ToDouble(values.First.ToString())));
                        values = values.Next;
                        lowData.Add(new KeyValuePair<string, double>(timeInterval.Name, Convert.ToDouble(values.First.ToString())));
                        values = values.Next;
                        closeData.Add(new KeyValuePair<string, double>(timeInterval.Name, Convert.ToDouble(values.First.ToString())));
                        values = values.Next;
                        volumeData.Add(new KeyValuePair<string, double>(timeInterval.Name, Convert.ToDouble(values.First.ToString())));
                        if (++i == 10)
                        {
                            break;
                        }
                    }
                    closeLines.Title = "Close";

                    closeLines.DependentValuePath = "Value";
                    closeLines.IndependentValuePath = "Key";
                    closeLines.ItemsSource = closeData;

                    LineSeries openLines = new LineSeries();
                    openLines.Title = "Open";
                    openLines.DependentValuePath = "Value";
                    openLines.IndependentValuePath = "Key";
                    openLines.ItemsSource = openData;
                    LineSeries highLines = new LineSeries();
                    highLines.Title = "High";
                    highLines.DependentValuePath = "Value";
                    highLines.IndependentValuePath = "Key";
                    highLines.ItemsSource = highData;
                    LineSeries lowLines = new LineSeries();
                    lowLines.Title = "Low";
                    lowLines.DependentValuePath = "Value";
                    lowLines.IndependentValuePath = "Key";
                    lowLines.ItemsSource = lowData;
                    LineSeries volumeLines = new LineSeries();
                    volumeLines.Title = "Volume";
                    volumeLines.DependentValuePath = "Value";
                    volumeLines.IndependentValuePath = "Key";
                    volumeLines.ItemsSource = volumeData;

                    List<LineSeries> lines = new List<LineSeries>();
                    lines.Add(lowLines);
                    lines.Add(highLines);
                    lines.Add(openLines);
                    lines.Add(closeLines);
                    Chart chart = new Chart();
                    chart.Series.Add(lowLines);
                    chart.Series.Add(highLines);
                    chart.Series.Add(openLines);
                    chart.Series.Add(closeLines);


                
                         if (this.Children.Contains(this.graphChart))
                         {
                             this.Children.Remove(this.graphChart);
                         }
                         this.graphChart = chart;
                         this.Children.Add(this.graphChart);
                 }));
                await Task.Delay(5000);
            }
        }
 
        public static void verticalSplitClick(object sender, RoutedEventArgs e)
        {

            //Button senderr = (Button)sender;
            MenuItem menuItem = (MenuItem)sender;
            String name = menuItem.GetValue(MenuItem.NameProperty) as String;
            String[] splitedName = name.Split('_');
            GridPanel grid = FindChild<GridPanel>(Application.Current.MainWindow, "grid_" + splitedName[1]);
            var mainGrid = (Grid)grid.FindName("grid");

            //BORDER



            int rowSpan = (int)grid.GetValue(Grid.RowSpanProperty);
            int columnSpan = (int)grid.GetValue(Grid.ColumnSpanProperty);
            if (rowSpan == 2)
            {
                menuItem.IsEnabled = false;
            }
            if (rowSpan != 1)
            {
                int row = (int)grid.GetValue(Grid.RowProperty);
                int column = (int)grid.GetValue(Grid.ColumnProperty);
                grid.SetValue(Grid.RowSpanProperty, rowSpan / 2);
                GridPanel newGrid = new GridPanel(id_counter.ToString(), rowSpan / 2, columnSpan, column, row + rowSpan / 2);

                mainGrid.Children.Add(newGrid);
            }
        }

        public static void horizontalSplitClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            String name = menuItem.GetValue(MenuItem.NameProperty) as String;
            String[] splitedName = name.Split('_');
            GridPanel grid = FindChild<GridPanel>(Application.Current.MainWindow, "grid_" + splitedName[1]);
            var mainGrid = (Grid)grid.FindName("grid");


            int rowSpan = (int)grid.GetValue(Grid.RowSpanProperty);
            int columnSpan = (int)grid.GetValue(Grid.ColumnSpanProperty);
            if (columnSpan == 2)
            {
                menuItem.IsEnabled = false;
            }
            if (columnSpan != 1)
            {
                int row = (int)grid.GetValue(Grid.RowProperty);
                int column = (int)grid.GetValue(Grid.ColumnProperty);
                grid.SetValue(Grid.ColumnSpanProperty, columnSpan / 2);
                GridPanel newGrid = new GridPanel(id_counter.ToString(), rowSpan, columnSpan / 2, column + columnSpan / 2, row);

                mainGrid.Children.Add(newGrid);
            }
        }
        public static T FindChild<T>(DependencyObject parent, string childName)
   where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        public static int id_counter = 3;

    }

}
