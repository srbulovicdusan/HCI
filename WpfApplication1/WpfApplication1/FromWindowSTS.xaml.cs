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

            if (!inputCheck)
            {
                MessageBox.Show("Please insert valid value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.InputNumber.Text = "";
                return;
            }

            string tempEquity = this.Equities.SelectedValue.ToString();
            string[] tempSplit = null;
            tempSplit = tempEquity.Split(new char[] { ' ' });
            equity = tempSplit[0];

            if (rb0.IsChecked.Value)
                temporal = "function=TIME_SERIES_INTRADAY";
            else if (rb1.IsChecked.Value)
            {
                temporal = "function=TIME_SERIES_DAILY";
                if (Adjusted1.IsChecked.Value)
                    temporal += "_ADJUSTED";
            }
            else if (rb2.IsChecked.Value)
            {
                temporal = "function=TIME_SERIES_WEEKLY";
                if (Adjusted2.IsChecked.Value)
                    temporal += "_ADJUSTED";
            }
            else
            {
                temporal = "function=TIME_SERIES_MONTHLY";
                if (Adjusted3.IsChecked.Value)
                    temporal += "_ADJUSTED";
            }


            // type of View
            if (rb4.IsChecked.Value)
                viewType = 1;
            else
            {
                viewType = 2;
                viewItem = 0;
            }


            string tempInterval = (string)((ComboBoxItem)this.CBTimeIntervals.SelectedValue).Name;
            tempSplit = null;
            tempSplit = tempInterval.Split(new char[] { 'n' });
            interval = tempSplit[1] + "min";



            // Ako je izabran INTRADAY -> jos jedan param 'interval'
            if (this.rb0.IsChecked.Value)
                urlParameters = String.Format("?{0}&symbol={1}&interval={2}&apikey=1ST174M77Q7QPYDW", temporal, equity, interval);
            else
                urlParameters = String.Format("?{0}&symbol={1}&apikey=1ST174M77Q7QPYDW", temporal, equity);

            Console.WriteLine(urlParameters);

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
        }

        private void rb1_Click(object sender, RoutedEventArgs e)
        {

            TimeInterval.Visibility = Visibility.Collapsed;
            CBTimeIntervals.Visibility = Visibility.Collapsed;
            Adjusted1.IsEnabled = true;
            Adjusted2.IsEnabled = false;
            Adjusted3.IsEnabled = false;

        }

        private void rb2_Click(object sender, RoutedEventArgs e)
        {
            TimeInterval.Visibility = Visibility.Collapsed;
            CBTimeIntervals.Visibility = Visibility.Collapsed;
            Adjusted1.IsEnabled = false;
            Adjusted2.IsEnabled = true;
            Adjusted3.IsEnabled = false;
        }

        private void rb3_Click(object sender, RoutedEventArgs e)
        {
            TimeInterval.Visibility = Visibility.Collapsed;
            CBTimeIntervals.Visibility = Visibility.Collapsed;
            Adjusted1.IsEnabled = false;
            Adjusted2.IsEnabled = false;
            Adjusted3.IsEnabled = true;
        }

        private void rb4_Click(object sender, RoutedEventArgs e)
        {
            TypeText.Visibility = Visibility.Visible;
            rb6.Visibility = Visibility.Visible;
            rb7.Visibility = Visibility.Visible;
            rb8.Visibility = Visibility.Visible;
            rb9.Visibility = Visibility.Visible;
            rb10.Visibility = Visibility.Visible;
        }

        private void rb5_Click(object sender, RoutedEventArgs e)
        {
            TypeText.Visibility = Visibility.Collapsed;
            rb6.Visibility = Visibility.Collapsed;
            rb7.Visibility = Visibility.Collapsed;
            rb8.Visibility = Visibility.Collapsed;
            rb9.Visibility = Visibility.Collapsed;
            rb10.Visibility = Visibility.Collapsed;
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
    }
}