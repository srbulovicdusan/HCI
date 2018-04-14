using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Currency
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        public Currency(string CurrencyCode, string CurrencyName)
        {
            this.CurrencyCode = CurrencyCode;
            this.CurrencyName = CurrencyName;
        }

    }

    class Market
    {
        public string MarketCode { get; set; }
        public string MarketName { get; set; }

        public Market(string MarketCode, string MarketName)
        {
            this.MarketCode = MarketCode;
            this.MarketName = MarketName;
        }
    }

    class Company
    {
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }

        public Company(string CompanyCode, string CompanyName)
        {
            this.CompanyCode = CompanyCode;
            this.CompanyName = CompanyName;
        }
    }
}
