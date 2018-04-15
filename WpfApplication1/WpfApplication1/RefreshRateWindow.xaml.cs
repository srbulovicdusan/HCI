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
    /// Interaction logic for RefreshRateWindow.xaml
    /// </summary>
    public partial class RefreshRateWindow : Window
    {

        public int refreshRate;

        public RefreshRateWindow()
        {
            InitializeComponent();
            refreshRate = 15000;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string tempRate = "";
            tempRate = (string)((ComboBoxItem)this.RefreshRate.SelectedValue).Name;
            string[] tempSplit = null;
            tempSplit = tempRate.Split(new char[] { 'c' });
            refreshRate = int.Parse(tempSplit[1]);
            refreshRate = refreshRate * 1000;


            this.Close();
        }
    }
}
