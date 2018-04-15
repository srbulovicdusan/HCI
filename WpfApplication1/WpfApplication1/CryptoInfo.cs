using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class CryptoInfo : DataParameters
    {
        public DataType data;
        public TimeSeries timeSeries;
        public string symbol { get; set; }
        public string marketCode { get; set; }
        public string marketName { get; set; }
        public override string getTimeSeriesKey()
        {
            switch (this.timeSeries)
            {
                case TimeSeries.INTRADAY:
                    return "Time Series (Digital Currency Intraday)";
                case TimeSeries.DAILY:
                    return "Time Series (Digital Currency Daily)";
                
                case TimeSeries.WEEKLY:
                    return "Time Series (Digital Currency Weekly)";
                
                case TimeSeries.MONTHLY:
                    return "Time Series (Digital Currency Monthly)";
                
                default:
                    return "error";

            }
        }
    }
}
