using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
namespace WpfApplication1
{
    public class StockInfo : DataParameters
    {

        public string interval { get; set; }
        public override string getTimeSeriesKey()
        {
            switch (this.timeSeries)
            {
                case TimeSeries.INTRADAY:
                    return "Time Series (" + this.interval + ")";
                case TimeSeries.DAILY:
                    return "Time Series (Daily)";
                case TimeSeries.DAILYADJUSTED:
                    return "Time Series (Daily)";
                case TimeSeries.WEEKLY:
                    return "Weekly Time Series";
                case TimeSeries.WEEKLYADJUSTED:
                    return "Weekly Time Series";
                case TimeSeries.MONTHLY:
                    return "Monthly Time Series";
                case TimeSeries.MONTHLYADJUSTED:
                    return "Monthly Time Series";
                default:
                    return "error";

            }
        }


    }



}
