using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace WpfApplication1
{
    public abstract class DataParameters
    {
        public DataType data;
        public ViewType view;
        public TimeSeries timeSeries;

        public int numOfPoints { get; set; }
        public string urlParameters { get; set; }
        public string symbol { get; set; }
        public string fullName { get; set; }
        public abstract string getTimeSeriesKey();
        

    }
    public enum DataType
    {

        [Description("open")]
        OPEN,
        [Description("high")]
        HIGH,
        [Description("low")]
        LOW,
        [Description("close")]
        CLOSE,
        [Description("volume")]
        VOLUME,
        [Description("adjusted close")]
        ADJUSTEDCLOSE,
        [Description("market cap")]
        MARKETCAP,
        [Description("price")]
        PRICE

    }
    public enum ViewType { GRAPH, TABLE, CURRENTVALUE }
    public enum TimeSeries
    {

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
