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
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace WpfApplication1
{
    class GridPanel : Grid
    {
        ContextMenu buttonMenu = new ContextMenu();
        Label valueLabel = new Label();
        Label valueLabelDescription = new Label();
        Label currency = new Label();
        Chart graphChart;
        bool isClearingActive;
        Task<Task> refreshGraphTask;
        DataParameters stockInfo;
        Label info = new Label();
        DataGrid dataGrid;
        StackPanel stackPanel = new StackPanel();
        string urlParameters;


        int refresh = 15000;


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

            this.valueLabel.FontSize = 60;
            this.valueLabel.VerticalAlignment = VerticalAlignment.Center;
            this.valueLabel.HorizontalAlignment = HorizontalAlignment.Center;

            //this.valueLabelDescription.FontSize = 35;
            this.valueLabelDescription.VerticalAlignment = VerticalAlignment.Center;
            this.valueLabelDescription.HorizontalAlignment = HorizontalAlignment.Center;

           // this.currency.FontSize = 35;
            this.currency.VerticalAlignment = VerticalAlignment.Center;
            this.currency.HorizontalAlignment = HorizontalAlignment.Center;
            this.stackPanel.Children.Add(this.valueLabelDescription);
            this.stackPanel.Children.Add(this.valueLabel);
            this.stackPanel.Children.Add(this.currency);
            this.stackPanel.VerticalAlignment = VerticalAlignment.Center;
            this.stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
           
            //border.Child = this;
            initializeComponents();
            //proba grafa

            this.Children.Add(this.info);

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
            horizontalSplit.Header = "Horizontal Split";
            horizontalSplit.Click += verticalSplitClick;
            if (rowSpan == 1)
            {
                horizontalSplit.IsEnabled = false;
            }
            MenuItem verticalSplit = new MenuItem();
            verticalSplit.Name = "vertical_" + id;
            verticalSplit.Header = "Vertical Split";
            verticalSplit.Click += horizontalSplitClick;
            if (colSpan == 1)
            {
                verticalSplit.IsEnabled = false;
            }
            MenuItem graphDisplay = new MenuItem();
            graphDisplay.Name = "stock";
            graphDisplay.Header = "Add Stock Monitoring";
            graphDisplay.Click += graphDisplayClick;
            MenuItem tableView = new MenuItem();
            tableView.Name = "crypto";
            tableView.Header = "Add Crypto Currency Monitoring";
            tableView.Click += CryptoClick;

            MenuItem clearView = new MenuItem();
            clearView.Name = "clear";
            clearView.Header = "Clear Window";
            clearView.Click += clearClick;
            clearView.IsEnabled = false;

            MenuItem currExchange = new MenuItem();
            currExchange.Name = "currency";
            currExchange.Header = "Currency Exchange";
            currExchange.Click += currExchangeClick;

            MenuItem refRate = new MenuItem();
            refRate.Name = "refRate";
            refRate.Header = "Refresh Rate";
            refRate.Click += refRateClick;

            MenuItem help = new MenuItem();
            help.Name = "help";
            help.Header = "Help";
            help.Click += helpClick;

          


            this.buttonMenu.Items.Add(graphDisplay);
            this.buttonMenu.Items.Add(tableView);
            this.buttonMenu.Items.Add(currExchange);
            //this.buttonMenu.Items.Add(new Separator());

            this.buttonMenu.Items.Add(horizontalSplit);
            this.buttonMenu.Items.Add(verticalSplit);
            this.buttonMenu.Items.Add(clearView);
            //this.buttonMenu.Items.Add(new Separator());
            this.buttonMenu.Items.Add(refRate);
            this.buttonMenu.Items.Add(help);

            
            
            b.ContextMenu = this.buttonMenu;
            b.Click += buttonClick;
            b.Content = path;
            this.Children.Add(b);
        }

        private async void currExchangeClick(object sender, RoutedEventArgs e)
        {
            ForeignExchangeWindow fxw = new ForeignExchangeWindow();
            fxw.ShowDialog();
            if (fxw.closed == true)
            {
                return;
            }

            foreach (MenuItem button in this.buttonMenu.Items)
            {

                if (button.Name == "stock" || button.Name == "crypto" || button.Name == "currency")
                {
                    button.IsEnabled = false;
                }
                if (button.Name == "clear")
                {
                    button.IsEnabled = true;
                }
            }
            this.urlParameters = fxw.urlParameters;
            this.refreshGraphTask = new Task<Task>(refreshCurrency);
            this.info.Content = "Currency exchange: \nFrom: " + fxw.from + "\nTo: " + fxw.to;
            this.refreshGraphTask.Start();

            await this.refreshGraphTask;
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
            

            FormWindowsSTS formWindow = new FormWindowsSTS();
            formWindow.ShowDialog();
            if (formWindow.closed == true)
            {
                return;
            }
            foreach (MenuItem button in this.buttonMenu.Items)
            {

                if (button.Name == "stock" || button.Name == "crypto" || button.Name == "currency")
                {
                    button.IsEnabled = false;
                }
                if (button.Name == "clear")
                {
                    button.IsEnabled = true;
                }
            }
            this.stockInfo = formWindow.stockInfo;
            await Dispatcher.BeginInvoke((Action)(() =>
            {
                this.info.Name = "info";



                if (this.stockInfo.view != ViewType.CURRENTVALUE) { 
                    this.info.Content = this.stockInfo.fullName + "\n" + EnumDescription(this.stockInfo.timeSeries);
                    if (this.stockInfo.timeSeries == TimeSeries.INTRADAY)
                    {

                        this.info.Content += " " + ((StockInfo)this.stockInfo).interval;
                    }
                }else
                {
                    this.info.Content = this.stockInfo.fullName + "\n" + EnumDescription(this.stockInfo.data);


                }
                this.info.Content += ", Currency: USD";
                this.info.SetValue(Label.VerticalAlignmentProperty, VerticalAlignment.Top);
            }));
            if (this.stockInfo.view == ViewType.GRAPH) {

                this.refreshGraphTask = new Task<Task>(refreshGraph);

            }else if (this.stockInfo.view == ViewType.TABLE)
            {
                this.refreshGraphTask = new Task<Task>(refreshTable);

            }else if (this.stockInfo.view == ViewType.CURRENTVALUE)
            {
                this.refreshGraphTask = new Task<Task>(refreshCurrentValue);
            }
      
            this.refreshGraphTask.Start();

            await this.refreshGraphTask;
            


        }

        private async Task refreshCurrentValue()
        {
            while (true)
            {

                StockApi api = new WpfApplication1.StockApi();
                Dictionary<string, dynamic> data = api.getData(this.stockInfo.urlParameters);


                if (data == null || !data.ContainsKey(this.stockInfo.getTimeSeriesKey()))
                {
                    MessageBox.Show("Service can not load required data at the moment! Please clear the window and try again!");
                    return;
                }


                int i = 0;
                await Dispatcher.BeginInvoke((Action)(() =>
                {

                    foreach (JProperty timeInterval in data[this.stockInfo.getTimeSeriesKey()])
                    {
                        if (this.isClearingActive == true)
                        {
                            return;
                        }
                        // do something with entry.Value or entry.Key
                        JToken value = timeInterval.First.First;
                        i++;
                        //this.valueLabel.Content = stockInfo.fullName + "\n" + stockInfo.data.ToString() + "\n";\\
                        if (!this.Children.Contains(this.stackPanel))
                        {
                            this.Children.Add(this.stackPanel);
                        }

                        //this.stackPanel.Height = this.ActualHeight - 20;
                        //this.stackPanel.Width = this.ActualWidth - 70;
                        //this.stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                        //this.stackPanel.VerticalAlignment = VerticalAlign
                        while (true)
                        {

                            if (value.Path.Contains(EnumDescription(this.stockInfo.data)) && this.stockInfo is StockInfo)
                            {
                                this.valueLabel.Content = value.First.ToString();
                                break;
                            }
                            else if (value.Path.Contains(EnumDescription(this.stockInfo.data)) && this.stockInfo is CryptoInfo)
                            {
                                if (this.stockInfo.data == DataType.MARKETCAP || this.stockInfo.data == DataType.VOLUME)
                                {
                                    this.valueLabel.Content = value.First.ToString();
                                    this.currency.Content = ((CryptoInfo)this.stockInfo).marketCode;
                                    break;
                                }
                                else if (value.Path.Contains(((CryptoInfo)this.stockInfo).marketCode))
                                {
                                    this.valueLabel.Content = value.First.ToString();
                                    break;
                                }
                            }
                            value = value.Next;
                        }
                        // values.First = 1. open value

                        if (i == stockInfo.numOfPoints)
                        {
                            break;
                        }
                        if (!this.Children.Contains(this.valueLabel))
                        {
                            this.Children.Add(this.valueLabel);
                        }

                    }

                   
                    


                }));
                if (this.isClearingActive == true)
                {
                    this.isClearingActive = false;
                    return;
                }
                await Task.Delay(refresh);
            }
        }
        
        public async Task refreshCurrency()
        {
            while (true)
            {

                StockApi api = new WpfApplication1.StockApi();
                Dictionary<string, dynamic> data = api.getData(this.urlParameters);


                if (data == null || !data.ContainsKey("Realtime Currency Exchange Rate"))
                {
                    if (data == null)
                    {
                        MessageBox.Show("data == null");
                    }
                    continue;
                }

                if (this.isClearingActive == true)
                {
                    return;
                }
                int i = 0;
                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (!this.Children.Contains(this.stackPanel))
                    {
                        this.Children.Add(this.stackPanel);
                    }
                    foreach (JProperty timeInterval in data["Realtime Currency Exchange Rate"])
                    {
                        if (this.isClearingActive == true)
                        {
                            return;
                        }
                        // do something with entry.Value or entry.Key
                        JToken value = timeInterval;
                        value = value.Next;
                        value = value.Next;
                        value = value.Next;
                        value = value.Next;
                        
                        this.valueLabel.Content = value.First.ToString();
                        break;
                        
                    }
                }));
                if (this.isClearingActive == true)
                {
                    this.isClearingActive = false;
                    return;
                }
                await Task.Delay(refresh);
                if (this.isClearingActive == true)
                {
                    this.isClearingActive = false;
                    return;
                }
            }
        
    }
        public async Task refreshTable()
        {
            while (true)
            {
                StockApi api = new StockApi();
                Dictionary<string, dynamic> data = api.getData(this.stockInfo.urlParameters);

                if (data == null || !data.ContainsKey(this.stockInfo.getTimeSeriesKey()))
                {
                   
                    MessageBox.Show("Service can not load required data at the moment! Please clear the window and try again!");
                    return;
                    //continue;
                }

                int i = 0;
                int j = 0;
                await Dispatcher.BeginInvoke((Action)(() =>
                {
                DataGrid dg = new DataGrid();
                DataTable table = new DataTable("Data history");

                DataColumn timeColumn = new DataColumn("time");
                table.Columns.Add(timeColumn);
                foreach (JProperty timeInterval in data[this.stockInfo.getTimeSeriesKey()])
                {
                    i++;

                    JToken value = timeInterval.First.First;

                    if (j == 0)
                    {
                        foreach (JToken child in timeInterval.First)
                        {
                            String token = (child.ToString());
                            String[] tokens = token.Split(':');
                            String columnValue = tokens[0].TrimStart('\"');
                            columnValue = columnValue.TrimEnd('\"');
                            string columnStr = columnValue.Split(' ')[1];
                            if (!table.Columns.Contains(columnStr))
                                {
                                    DataColumn tempColumn = new DataColumn(columnStr);
                                    table.Columns.Add(tempColumn);
                                }


                                //table.Columns.Add(new DataColumn(columnValue.Split(' ')[1]));

                            }
                        }


                        DataRow row = table.NewRow();
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column == timeColumn)
                            {
                                row[column] = timeInterval.Name.ToString();

                            }
                            else
                            {
                                row[column] = Convert.ToString(value.First);
                                value = value.Next;

                            }

                        }

                        table.Rows.Add(row);

                        if (i == stockInfo.numOfPoints)
                        {
                            break;
                        }
                        j++;
                    }
                    if (table.HasErrors == true)
                    {
                        MessageBox.Show("error");
                    }
                    dg.ItemsSource = table.DefaultView;

                    DataGridSettings(dg);

                    if (this.isClearingActive == true)
                    {

                        return;
                    }
                    if (this.Children.Contains(this.dataGrid))
                    {
                        this.Children.Remove(this.dataGrid);
                    }
                    this.dataGrid = dg;
                    this.Children.Add(this.dataGrid);
                }));

                if (this.isClearingActive == true)
                {
                    this.isClearingActive = false;
                    return;
                }
                await Task.Delay(refresh);
            }
        }

        public void DataGridSettings(DataGrid dg)
        {
            dg.IsReadOnly = true;
            dg.CanUserResizeColumns = true;
            dg.CanUserResizeRows = true;
            dg.Height = this.ActualHeight - 20;
            dg.Width = this.ActualWidth - 70;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.HorizontalAlignment = HorizontalAlignment.Center;

            dg.MaxWidth = 300;
            dg.MaxHeight = 300;
        }

        public async Task refreshGraph()
        {
            while (true)
            {

                StockApi api = new WpfApplication1.StockApi();
                Dictionary<string, dynamic> data =  api.getData(this.stockInfo.urlParameters);
                

                if (data == null || !data.ContainsKey(this.stockInfo.getTimeSeriesKey()))
                {
                    MessageBox.Show("Service can not load required data at the moment! Please clear the window and try again!");
                    return;
                }



                List<KeyValuePair<string, double>> dataValues = new List<KeyValuePair<string, double>>();
                int i = 0;
                await Dispatcher.BeginInvoke((Action)(() =>
                {

                    foreach (JProperty timeInterval in data[this.stockInfo.getTimeSeriesKey()])
                    {

                        // do something with entry.Value or entry.Key
                        JToken value = timeInterval.First.First;
                        i++;

                        while (true)
                        {
                            
                            if (value.Path.Contains(EnumDescription(this.stockInfo.data)) && this.stockInfo is StockInfo)
                            {
                                dataValues.Add(new KeyValuePair<string, double>(timeInterval.Name, Convert.ToDouble(value.First.ToString())));
                                break;
                            }else if (value.Path.Contains(EnumDescription(this.stockInfo.data))  && this.stockInfo is CryptoInfo)
                            {
                                if (this.stockInfo.data == DataType.MARKETCAP || this.stockInfo.data == DataType.VOLUME)
                                {
                                    dataValues.Add(new KeyValuePair<string, double>(timeInterval.Name, Convert.ToDouble(value.First.ToString())));
                                    break;
                                }
                                else if (value.Path.Contains(((CryptoInfo)this.stockInfo).marketCode))
                                {
                                    dataValues.Add(new KeyValuePair<string, double>(timeInterval.Name, Convert.ToDouble(value.First.ToString())));
                                    break;
                                }
                            }
                            value = value.Next;
                        }
                        // values.First = 1. open value
                        
                        if (i == stockInfo.numOfPoints)
                        {
                            break;
                        }
                    }

                    LineSeries dataLineSeries = new LineSeries();
                    dataLineSeries.Title = EnumDescription(stockInfo.data);
                    dataLineSeries.DependentValuePath = "Value";
                    dataLineSeries.IndependentValuePath = "Key";
                    dataLineSeries.ItemsSource = dataValues;
                        
         
                    Chart chart = new Chart();
                    
             
                    chart.Series.Add(dataLineSeries);
                    
                    
                    

                    if (this.isClearingActive == true)
                    {
                       
                        return;
                    }
                    if (this.Children.Contains(this.graphChart))
                    {
                        this.Children.Remove(this.graphChart);
                    }
                    this.graphChart = chart;
                    this.Children.Add(this.graphChart);

                }));
                if (this.isClearingActive == true)
                {
                    this.isClearingActive = false;
                    return;
                }
                await Task.Delay(refresh);
            }
        }
        private async void CryptoClick(object sender, RoutedEventArgs e)
        {
            

            FormWindowCrypto formWindow = new FormWindowCrypto();
            formWindow.ShowDialog();
            if (formWindow.closed == true)
            {
                return;
            }
            foreach (MenuItem button in this.buttonMenu.Items)
            {

                if (button.Name == "crypto" || button.Name == "stock" || button.Name == "currency")
                {
                    button.IsEnabled = false;
                }
                if (button.Name == "clear")
                {
                    button.IsEnabled = true;
                }
            }
            this.stockInfo = formWindow.cryptoInfo;
            await Dispatcher.BeginInvoke((Action)(() =>
            {
                this.info.Name = "info";
                this.info.Content = this.stockInfo.fullName + "  " + EnumDescription((this.stockInfo).timeSeries);
                this.info.SetValue(Label.VerticalAlignmentProperty, VerticalAlignment.Top);
            }));
            if (this.stockInfo.view == ViewType.CURRENTVALUE)
            {
                this.refreshGraphTask = new Task<Task>(refreshCurrentValue);

            }
            else if (this.stockInfo.view == ViewType.GRAPH)
            {

                this.refreshGraphTask = new Task<Task>(refreshGraph);

            }
            else if (this.stockInfo.view == ViewType.TABLE)
            {
                this.refreshGraphTask = new Task<Task>(refreshTable);

            }

            this.refreshGraphTask.Start();

            await this.refreshGraphTask;


        }

        private async void clearClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            //String name = menuItem.GetValue(MenuItem.NameProperty) as String;
            //String[] splitedName = name.Split('_');
            //GridPanel gp = FindChild<GridPanel>(Application.Current.MainWindow, "grid_" + splitedName[1]);
            //this.refreshGraphTask.Dispose();
            this.isClearingActive = true;
            for (int i = 0; i< this.Children.Count; i++) 
            {
                if (this.Children[i] is Chart || this.Children[i] is DataGrid || this.Children[i] is Label || this.Children[i] is StackPanel)
                {
                    await Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (this.Children[i] is Label)
                        {
                            ((Label)this.Children[i]).Content = "";
                           
                        }else
                        {
                            this.Children.RemoveAt(i);

                        }
                        return;
                    }));
                }
            }
            foreach (MenuItem button in this.buttonMenu.Items)
            {
                if (button.Name == "stock" || button.Name == "crypto" || button.Name == "currency")
                {
                    button.IsEnabled = true;
                }
            }

            menuItem.IsEnabled = false;
            //((MenuItem)buttonMenu.Items.GetItemAt(2)).IsEnabled = true;
        }


        public void refRateClick(object sender, RoutedEventArgs e)
        {
            RefreshRateWindow rw = new RefreshRateWindow();
            rw.ShowDialog();

            this.refresh = rw.refreshRate;
        }

        public void helpClick(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            hw.ShowDialog();
        }



        public void verticalSplitClick(object sender, RoutedEventArgs e)
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

        public void horizontalSplitClick(object sender, RoutedEventArgs e)
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
        public static string EnumDescription(Enum FruitType)
        {
            FieldInfo fi = FruitType.GetType().GetField(FruitType.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return FruitType.ToString();
            }
        }
        public static int id_counter = 3;

    }

}
