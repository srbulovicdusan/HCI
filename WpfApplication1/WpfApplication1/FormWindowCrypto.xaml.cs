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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for FormWindowCrypto.xaml
    /// </summary>
    public partial class FormWindowCrypto : Window
    {

        private List<Currency> _currencies = new List<Currency>();
        private List<Market> _markets = new List<Market>();
        public static string urlParameters = "";
        public static int refreshRate = 10;

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
            string currency, market, temporal, tempRate = "";


            currency = this.Currencies.SelectedValue.ToString();
            currency = currency.Split(new char[] { ' ' })[0];
            market = this.Markets.SelectedValue.ToString();
            market = market.Split(new char[] { ' ' })[0];

            if (rb1.IsChecked.Value)
                temporal = "function=TIME_SERIES_DAILY";
            else if (rb2.IsChecked.Value)
                temporal = "function=TIME_SERIES_WEEKLY";
            else
                temporal = "function=TIME_SERIES_MONTHLY";

            tempRate = (string)((ComboBoxItem)this.RefreshRate.SelectedValue).Name;
            string[] tempSplit = null;
            tempSplit = tempRate.Split(new char[] { 'c' });
            refreshRate = int.Parse(tempSplit[1]);

            urlParameters = String.Format("?{0}&symbol={1}&market={2}&apikey=1ST174M77Q7QPYDW", temporal, currency, market);
            Console.WriteLine(urlParameters);

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
    }
}