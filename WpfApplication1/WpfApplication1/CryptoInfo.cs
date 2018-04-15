using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class CryptoInfo : DataParameters
    {
        public DataType data;
        public TimeSeries timeSeries;
        public string symbol { get; set; }
        public string marketCode { get; set; }
        public string marketName { get; set; }
        public override string getTimeSeriesKey()
        {
            return "aaa";
        }
    }
}
