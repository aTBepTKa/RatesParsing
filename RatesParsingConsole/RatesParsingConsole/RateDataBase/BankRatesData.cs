using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.RateDataBase
{
    /// <summary>
    /// Данные курсов валют банка.
    /// </summary>
    class BankRatesData
    {
        /// <summary>
        /// Курсы валют банка.
        /// </summary>
        public List<CurrencyData> CurrencyDatas { get; set; }

        /// <summary>
        /// Страна расположения банка.
        /// </summary>
        public CountryData BankCountry { get; set; }
    }
}
