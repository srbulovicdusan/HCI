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
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {

        // mode=1 (Beginner's mode)
        // mode=2 (Advanced mode)
        // mode=0 (nothing checed -> open Main Window recimo)
        public int mode;

        public StartupWindow()
        {
            InitializeComponent();

            this.StartInfo.Text = "This is a quick guide for using this application." +
                "Main Window is a blank page with a Control Button in the right top " +
                "corner. When button is clicked, drop-down menu opens. All of the " +
                "features are stored in the menu: Screen Split, Add Monitoring, Remove " +
                "Monitoring (Clear window), Options, Help. For more infomations, click on " +
                "Help from Control Button drop - down menu.";
            
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (rb0.IsChecked.Value)
                mode = 1;
            else if (rb1.IsChecked.Value)
                mode = 2;
            else
                mode = 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            this.FinishButton.IsEnabled = true;

        }
    }
}
