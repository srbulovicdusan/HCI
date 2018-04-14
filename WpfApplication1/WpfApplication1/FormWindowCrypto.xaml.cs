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
    /// Interaction logic for FormWindowCrypto.xaml
    /// </summary>
    public partial class FormWindowCrypto : Window
    {

        private List<Currency> _currencies = new List<Currency>();
        private List<Market> _markets = new List<Market>();
        public string urlParameters = "";
        public bool inputCheck;


        // type=1 (GraphView)  
        // type=2 (TableView)
        public int viewType;

        // viewItem=0 (ako je Table View ipak)
        // viewItem=1 (Price)
        // viewItem=2 (Volume)
        // viewItem=3 (MarketCap)
        // Price is checked by default
        public int viewItem = 1;


        public FormWindowCrypto()
        {
            LoadCurrenciesData();
            LoadMarketsData();

            InitializeComponent();

            foreach (Currency c in _currencies)
            {
                string item = "";
                item = item + c.CurrencyCode + " (" + c.CurrencyName + ")";
                Currencies.Items.Add(item);
            }

            foreach (Market m in _markets)
            {
                string item = "";
                item = item + m.MarketCode + " (" + m.MarketName + ")";
                Markets.Items.Add(item);

            }

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string currency, market, temporal = "";

            if (!inputCheck)
            {
                MessageBox.Show("Please insert valid value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.InputNumber.Text = "";
                return;
            }

            currency = this.Currencies.SelectedValue.ToString();
            currency = currency.Split(new char[] { ' ' })[0];
            market = this.Markets.SelectedValue.ToString();
            market = market.Split(new char[] { ' ' })[0];

            if (rb0.IsChecked.Value)
                temporal = "function=DIGITAL_CURRENCY_INTRADAY";
            else if (rb1.IsChecked.Value)
                temporal = "function=DIGITAL_CURRENCY_DAILY";
            else if (rb2.IsChecked.Value)
                temporal = "function=DIGITAL_CURRENCY_WEEKLY";
            else
                temporal = "function=DIGITAL_CURRENCY_MONTHLY";

            if (rb4.IsChecked.Value)
            {
                viewType = 1;
            }
            else
            {
                viewItem = 0;
                viewType = 2;
            }


            urlParameters = String.Format("?{0}&symbol={1}&market={2}&apikey=1ST174M77Q7QPYDW", temporal, currency, market);
            Console.WriteLine(urlParameters);
            Console.WriteLine(viewItem);

            this.Close();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadCurrenciesData()
        {
            using (TextFieldParser parser = new TextFieldParser(@"../../data/digital_currency_list.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                // skip header
                parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    _currencies.Add(new Currency(fields[0], fields[1]));
                }
            }

        }

        private void LoadMarketsData()
        {
            using (TextFieldParser parser = new TextFieldParser(@"../../data/physical_currency_list.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                // skip header
                parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    _markets.Add(new Market(fields[0], fields[1]));
                }
            }

        }

        private void rb4_Click(object sender, RoutedEventArgs e)
        {
            TypeText.Visibility = Visibility.Visible;
            rb6.Visibility = Visibility.Visible;
            rb7.Visibility = Visibility.Visible;
            rb8.Visibility = Visibility.Visible;

        }

        private void rb5_Click(object sender, RoutedEventArgs e)
        {
            TypeText.Visibility = Visibility.Collapsed;
            rb6.Visibility = Visibility.Collapsed;
            rb7.Visibility = Visibility.Collapsed;
            rb8.Visibility = Visibility.Collapsed;

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