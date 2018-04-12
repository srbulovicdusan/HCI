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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for FormWindowsSTS.xaml
    /// </summary>
    public partial class FormWindowsSTS : Window
    {

        public  string urlParameters = "";
        public  int refreshRate = 10;

        public FormWindowsSTS()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string equity, temporal, tempRate = "";


            equity = (string)((ComboBoxItem)this.Equity.SelectedValue).Name;

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

            urlParameters = String.Format("?{0}&symbol={1}&apikey=1ST174M77Q7QPYDW", temporal, equity);

            Console.WriteLine(urlParameters);

            this.Close();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}