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
using System.Windows.Shapes;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for FormWindowsSTS.xaml
    /// </summary>
    public partial class FormWindowsSTS : Window
    {

        private List<Company> _companies = new List<Company>();
        public string urlParameters = "";
        public int refreshRate = 10;
        public bool inputCheck;


        // type=1 (GraphView)
        // type=2 (TableView)
        public int viewType;

        // item=0 (ako je TableView ipak)
        // item=1 (open)
        // item=2 (high)
        // item=3 (low)
        // item=4 (close)
        // item=5 (volume)
        // Open is checked by default
        public int viewItem = 1;
        public StockInfo stockInfo = new StockInfo();

        // true -> prikaz samo currValue
        // false -> normalno
        public bool currValue;



        public FormWindowsSTS()
        {

            LoadCompaniesData();
            _companies.Sort((x, y) => x.CompanyName.CompareTo(y.CompanyName));

            InitializeComponent();

            foreach (Company c in _companies)
            {
                string item = "";
                item = item + c.CompanyCode + " (" + c.CompanyName + ")";
                this.Equities.Items.Add(item);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string equity, temporal, interval = "";

            if (this.Current.IsChecked.Value)
            {
                currValue = true;
            }
            else
            {
                currValue = false;
            }

            if (!inputCheck)
            {
                MessageBox.Show("Please insert valid value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.InputNumber.Text = "";
                this.InputNumber.BorderBrush = Brushes.Red;
                this.InputError.Visibility = Visibility.Visible;
                return;
            }

            string tempEquity = this.Equities.SelectedValue.ToString();
            this.stockInfo.fullName = tempEquity;

            string[] tempSplit = null;
            tempSplit = tempEquity.Split(new char[] { ' ' });

            equity = tempSplit[0];
            this.stockInfo.symbol = equity;

            if (rb0.IsChecked.Value)
            {
                temporal = "function=TIME_SERIES_INTRADAY";
                this.stockInfo.timeSeries = TimeSeries.INTRADAY;
            }
            else if (rb1.IsChecked.Value)
            {
                temporal = "function=TIME_SERIES_DAILY";
                this.stockInfo.timeSeries = TimeSeries.DAILY;
                if (Adjusted1.IsChecked.Value)
                {
                    temporal += "_ADJUSTED";
                    this.stockInfo.timeSeries = TimeSeries.DAILYADJUSTED;
                }
            }
            else if (rb2.IsChecked.Value)
            {
                temporal = "function=TIME_SERIES_WEEKLY";
                this.stockInfo.timeSeries = TimeSeries.WEEKLY;

                if (Adjusted2.IsChecked.Value)
                {
                    temporal += "_ADJUSTED";
                    this.stockInfo.timeSeries = TimeSeries.WEEKLYADJUSTED;
                }
            }
            else
            {
                temporal = "function=TIME_SERIES_MONTHLY";
                this.stockInfo.timeSeries =TimeSeries.MONTHLY;

                if (Adjusted3.IsChecked.Value)
                {

                    temporal += "_ADJUSTED";
                    this.stockInfo.timeSeries = TimeSeries.MONTHLYADJUSTED;

                }
            }


            // type of View
            if (rb4.IsChecked.Value)
            { 
                viewType = 1;
                this.stockInfo.view = ViewType.GRAPH;
            }
            else
            {
                this.stockInfo.view = ViewType.TABLE;
                viewType = 2;
                viewItem = 0;
            }
            if (this.Current.IsChecked == true)
            {
                this.stockInfo.view = ViewType.CURRENTVALUE;
            }


            string tempInterval = (string)((ComboBoxItem)this.CBTimeIntervals.SelectedValue).Name;
            tempSplit = null;
            tempSplit = tempInterval.Split(new char[] { 'n' });
            interval = tempSplit[1] + "min";
            this.stockInfo.interval = interval;
            



            // Ako je izabran INTRADAY -> jos jedan param 'interval'
            if (this.rb0.IsChecked.Value)
                urlParameters = String.Format("?{0}&symbol={1}&interval={2}&apikey=1ST174M77Q7QPYDW", temporal, equity, interval);
            else
                urlParameters = String.Format("?{0}&symbol={1}&apikey=1ST174M77Q7QPYDW", temporal, equity);

            Console.WriteLine(urlParameters);
            this.stockInfo.urlParameters = urlParameters;
            this.stockInfo.data = DataType.OPEN;
            if (rb6.IsChecked == true)
            {
                this.stockInfo.data = DataType.OPEN;
            }
            else if(rb7.IsChecked == true)
            {
                this.stockInfo.data = DataType.HIGH;
            }
            else if(rb8.IsChecked == true)
            {
                this.stockInfo.data = DataType.LOW;
            }
            else if (rb9.IsChecked == true)
            {
                this.stockInfo.data = DataType.CLOSE;
            }
            else if (rb10.IsChecked == true)
            {
                this.stockInfo.data = DataType.VOLUME;
            }
            
             else if( rb11.IsChecked == true)
             {
                this.stockInfo.data = DataType.ADJUSTEDCLOSE;
              }
             

            this.stockInfo.numOfPoints = Int32.Parse(this.InputNumber.Text);
            this.Close();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void rb0_Click(object sender, RoutedEventArgs e)
        {
            this.TimeInterval.Visibility = Visibility.Visible;
            this.CBTimeIntervals.Visibility = Visibility.Visible;
            this.Adjusted1.IsEnabled = false;
            this.Adjusted2.IsEnabled = false;
            this.Adjusted3.IsEnabled = false;

            //this.rb11.Visibility = Visibility.Collapsed;
            

            this.Adjusted1.IsChecked = false;
            this.Adjusted2.IsChecked = false;
            this.Adjusted3.IsChecked = false;

            this.rb6.IsChecked = true;
        }

        private void rb1_Click(object sender, RoutedEventArgs e)
        {

            this.TimeInterval.Visibility = Visibility.Collapsed;
            this.CBTimeIntervals.Visibility = Visibility.Collapsed;
            this.Adjusted1.IsEnabled = true;
            this.Adjusted2.IsEnabled = false;
            this.Adjusted3.IsEnabled = false;
           
            this.Adjusted2.IsChecked = false;
            this.Adjusted3.IsChecked = false;

            this.rb6.IsChecked = true;


        }

        private void rb2_Click(object sender, RoutedEventArgs e)
        {
            this.TimeInterval.Visibility = Visibility.Collapsed;
            this.CBTimeIntervals.Visibility = Visibility.Collapsed;
            this.Adjusted1.IsEnabled = false;
            this.Adjusted2.IsEnabled = true;
            this.Adjusted3.IsEnabled = false;

            this.Adjusted1.IsChecked = false;
            this.Adjusted3.IsChecked = false;

            this.rb6.IsChecked = true;
        }

        private void rb3_Click(object sender, RoutedEventArgs e)
        {
            this.TimeInterval.Visibility = Visibility.Collapsed;
            this.CBTimeIntervals.Visibility = Visibility.Collapsed;
            this.Adjusted1.IsEnabled = false;
            this.Adjusted2.IsEnabled = false;
            this.Adjusted3.IsEnabled = true;

            this.Adjusted1.IsChecked = false;
            this.Adjusted2.IsChecked = false;

            this.rb6.IsChecked = true;
        }

        private void rb4_Click(object sender, RoutedEventArgs e)
        {
            this.TypeText.Visibility = Visibility.Visible;
            this.rb6.Visibility = Visibility.Visible;
            this.rb7.Visibility = Visibility.Visible;
            this.rb8.Visibility = Visibility.Visible;
            this.rb9.Visibility = Visibility.Visible;
            this.rb10.Visibility = Visibility.Visible;
            this.rb11.Visibility = Visibility.Visible;

            if (this.Adjusted1.IsChecked.Value || this.Adjusted2.IsChecked.Value || this.Adjusted3.IsChecked.Value)
            {
                //rb11.Visibility = Visibility.Visible;
                this.rb11.IsEnabled = true;
               
            }
            else
            {
                //rb11.Visibility = Visibility.Collapsed;
                this.rb11.IsEnabled = false;
               
            }

        }

        private void rb5_Click(object sender, RoutedEventArgs e)
        {
            this.TypeText.Visibility = Visibility.Collapsed;
            this.rb6.Visibility = Visibility.Collapsed;
            this.rb7.Visibility = Visibility.Collapsed;
            this.rb8.Visibility = Visibility.Collapsed;
            this.rb9.Visibility = Visibility.Collapsed;
            this.rb10.Visibility = Visibility.Collapsed;
            this.rb11.Visibility = Visibility.Collapsed;
            
        }

        private void rb6_Click(object sender, RoutedEventArgs e)
        {
            viewItem = 1;
        }

        private void rb7_Click(object sender, RoutedEventArgs e)
        {
            viewItem = 2;
        }

        private void rb8_Click(object sender, RoutedEventArgs e)
        {
            viewItem = 3;
        }

        private void rb9_Click(object sender, RoutedEventArgs e)
        {
            viewItem = 4;
        }

        private void rb10_Click(object sender, RoutedEventArgs e)
        {
            viewItem = 5;
        }

        private void LoadCompaniesData()
        {
            using (TextFieldParser parser = new TextFieldParser(@"../../data/companylist.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                // skip header
                parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    _companies.Add(new Company(fields[0], fields[1]));
                }
            }

        }

        private void InputNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

            inputCheck = IsTextAllowed(((TextBox)sender).Text);

            if (!inputCheck)
            {
                this.InputNumber.BorderBrush = Brushes.Red;
                this.InputError.Visibility = Visibility.Visible;
            }
        }



        private bool IsTextAllowed(string text)
        {
            bool ret;
            Regex regex = new Regex("[^0-9.-]+");

            ret = !regex.IsMatch(text);

            if (ret)
            {
                if (text.Equals(""))
                    return false;

                int num = int.Parse(text);

                if (num > 100)
                {
                    return false;
                }
            }

            if (ret)
            {
                this.InputError.Visibility = Visibility.Hidden;
                this.InputNumber.ClearValue(TextBox.BorderBrushProperty);
            }


            return ret;
        }

        private void Adjusted1_Checked(object sender, RoutedEventArgs e)
        {
            //this.rb11.Visibility = Visibility.Visible;
            this.rb11.IsEnabled = true;
          
        }

        private void Adjusted2_Checked(object sender, RoutedEventArgs e)
        {
            //this.rb11.Visibility = Visibility.Visible;
            this.rb11.IsEnabled = true;

        }

        private void Adjusted3_Checked(object sender, RoutedEventArgs e)
        {
            // this.rb11.Visibility = Visibility.Visible;
            this.rb11.IsEnabled = true;

        }

        private void Adjusted1_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.rb11.Visibility = Visibility.Collapsed;
            this.rb11.IsEnabled = false;
           
        }

        private void Adjusted2_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.rb11.Visibility = Visibility.Collapsed;
            this.rb11.IsEnabled = false;

        }

        private void Adjusted3_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.rb11.Visibility = Visibility.Collapsed;
            this.rb11.IsEnabled = false;

        }

        private void Current_Checked(object sender, RoutedEventArgs e)
        {
            this.rb0.IsChecked = true;
            this.rb0.IsEnabled = false;
            this.rb1.IsEnabled = false;
            this.rb2.IsEnabled = false;
            this.rb3.IsEnabled = false;
            this.Adjusted1.IsChecked = false;
            this.Adjusted2.IsChecked = false;
            this.Adjusted3.IsChecked = false;
            this.Adjusted1.IsEnabled = false;
            this.Adjusted2.IsEnabled = false;
            this.Adjusted3.IsEnabled = false;
            this.InputNumber.Text = "1";
            this.InputNumber.IsEnabled = false;
            this.InputNumber.Background = Brushes.DarkGray;
            this.CBTimeIntervals.IsEnabled = false;
            this.rb11.IsEnabled = false;
            this.rb6.IsChecked = true;
            this.rb4.IsEnabled = false;
            this.rb5.IsEnabled = false;
            

        }

        private void Current_Unchecked(object sender, RoutedEventArgs e)
        {
            this.rb0.IsEnabled = true;
            this.rb1.IsEnabled = true;
            this.rb2.IsEnabled = true;
            this.rb3.IsEnabled = true;
            if (this.rb1.IsChecked.Value)
                this.Adjusted1.IsEnabled = true;
            else if (this.rb2.IsChecked.Value)
                this.Adjusted2.IsEnabled = true;
            else if (this.rb3.IsChecked.Value)
                this.Adjusted3.IsEnabled = true;

            this.InputNumber.IsEnabled = true;
            
            this.InputNumber.ClearValue(TextBox.BackgroundProperty);
            this.InputNumber.Text = "";
            this.CBTimeIntervals.IsEnabled = true;

            this.CBTimeIntervals.Background = Brushes.DarkGray;

            this.rb4.IsEnabled = true;
            this.rb5.IsEnabled = true;

        }
    }
}