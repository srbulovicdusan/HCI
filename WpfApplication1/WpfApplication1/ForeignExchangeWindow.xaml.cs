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
    /// Interaction logic for ForeignExchangeWindow.xaml
    /// </summary>
    public partial class ForeignExchangeWindow : Window
    {

        private List<Currency> _cryptoCurrencies = new List<Currency>();
        private List<Currency> _physicalCurrencies = new List<Currency>();
        public string urlParameters = "?function=CURRENCY_EXCHANGE_RATE";
        public string apikey = "1ST174M77Q7QPYDW";
        public string from;
        public string to;
        public bool closed = true;

        public ForeignExchangeWindow()
        {

            LoadCurrenciesData();

            InitializeComponent();

            foreach (Currency c in _cryptoCurrencies)
            {
                string item = "";
                item = item + c.CurrencyCode + " (" + c.CurrencyName + ")";
                FromCryptoCurrency.Items.Add(item);
                ToCryptoCurrency.Items.Add(item);
            }

            foreach (Currency c in _physicalCurrencies)
            {
                string item = "";
                item = item + c.CurrencyCode + " (" + c.CurrencyName + ")";
                FromPhysicalCurrency.Items.Add(item);
                ToPhysicalCurrency.Items.Add(item);
            }


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
                    _cryptoCurrencies.Add(new Currency(fields[0], fields[1]));
                }
            }

            using (TextFieldParser parser = new TextFieldParser(@"../../data/physical_currency_list.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                // skip header
                parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    _physicalCurrencies.Add(new Currency(fields[0], fields[1]));
                }
            }
        }

        private void rb0_Click(object sender, RoutedEventArgs e)
        {
            this.FromCryptoCurrency.Visibility = Visibility.Collapsed;
            this.FromPhysicalCurrency.Visibility = Visibility.Visible;

        }

        private void rb1_Click(object sender, RoutedEventArgs e)
        {
            this.FromCryptoCurrency.Visibility = Visibility.Visible;
            this.FromPhysicalCurrency.Visibility = Visibility.Collapsed;

        }

        private void rb2_Click(object sender, RoutedEventArgs e)
        {
            this.ToCryptoCurrency.Visibility = Visibility.Collapsed;
            this.ToPhysicalCurrency.Visibility = Visibility.Visible;

        }

        private void rb3_Click(object sender, RoutedEventArgs e)
        {
            this.ToCryptoCurrency.Visibility = Visibility.Visible;
            this.ToPhysicalCurrency.Visibility = Visibility.Collapsed;

        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            string fromCurr, toCurr, tempCurr = null;
            string[] tempSplit = null;


            if (rb0.IsChecked.Value)
            {
                tempCurr = this.FromPhysicalCurrency.SelectedValue.ToString();
                from = tempCurr;
                tempSplit = tempCurr.Split(new char[] { ' ' });
                fromCurr = tempSplit[0];
                
            }
            else
            {
                tempCurr = this.FromCryptoCurrency.SelectedValue.ToString();
                from = tempCurr;
                tempSplit = tempCurr.Split(new char[] { ' ' });
                fromCurr = tempSplit[0];
                
            }

            if (rb2.IsChecked.Value)
            {
                tempCurr = this.ToPhysicalCurrency.SelectedValue.ToString();
                to = tempCurr;
                tempSplit = tempCurr.Split(new char[] { ' ' });
                toCurr = tempSplit[0];
                
            }
            else
            {
                tempCurr = this.ToCryptoCurrency.SelectedValue.ToString();
                to = tempCurr;
                tempSplit = tempCurr.Split(new char[] { ' ' });
                toCurr = tempSplit[0];
                
            }


            urlParameters = urlParameters + "&from_currency=" + fromCurr + "&to_currency=" + toCurr + "&apikey=" + apikey;

            this.closed = false;
            this.Close();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.closed = true;
            this.Close();
        }

    }


}
