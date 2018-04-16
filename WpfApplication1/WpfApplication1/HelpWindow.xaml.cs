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
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();

            this.StartInfo.Text =
                "Main Window is a blank page with a Control Button in the right top " +
                "corner. When button is clicked, drop-down menu opens. All of the " +
                "features are stored in the menu: Screen Split, Add Monitoring, Clear " +
                "Window, Refresh Rate Settings, Help. \n\n" + 
                "Context of Control button menu: ";
            
            this.Description.Text =
                "\nIf you click on Add Stock Monitoring, Add Crypto Currency Monitoring, " +
                "Foreign Exchange, or Refresh Rate, you will see the form with parameters. " +
                "You can choose table or graph data view. For crypto and stock data you can " +
                "choose to see only current value. Also you can choose temporal resolution for data " +
                "history which you can see on graph or in table. " +
                "When monitoring Stock market in Intraday mode, you can choose interval between two consecutive data points" +
                " (1min, 5min, 15min, 30min, 1hour). " +
                "\nIn Currency Exchange Window, you are able to convert from one currency to another. Both Crypto and Physical " +
                "currencies are supported. \nIn Refresh Rate Window, you can specify refreshing ratio of graphic elements.";

        }
    }
}
