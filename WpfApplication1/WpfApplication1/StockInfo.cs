using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
namespace WpfApplication1
{
    public class StockInfo
    {
        public string symbol { get; set; }
        public string fullName { get; set; }
        public string interval;
        public DataType data;
        public ViewType view;
        public StockType stock;
        public int numOfPoints { get; set; }
        public string urlParameters { get; set; }


    }
    public enum DataType {

        [Description("1. open")]
        OPEN,
        [Description("2. high")]
        HIGH,
        [Description("3. low")]
        LOW,
        [Description("4. close")]
        CLOSE,
        [Description("5. volume")]
        VOLUME }
    public enum ViewType { GRAPH, TABLE }
    public enum StockType {

        [Description("Intraday")]
        INTRADAY,
        [Description("Time Series (Daily)")]
        DAILY,
        [Description("Time Series (Daily)")]
        DAILYADJUSTED,
        [Description("Weekly Time Series")]
        WEEKLY,
        [Description("Weekly Adjusted Time Series")]
        WEEKLYADJUSTED,
        [Description("Monthly Time Series")]
        MONTHLY,
        [Description("Monthly Adjusted Time Series")]
        MONTHLYADJUSTED
    }



}
