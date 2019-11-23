using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.Models
{
    /// <summary>
    /// Данные курсов валют банка.
    /// </summary>
    class BankRatesDataModel
    {
        /// <summary>
        /// Курсы валют банка.
        /// </summary>
        public IEnumerable<CurrencyDataModel> CurrencyDatas { get; set; }

        /// <summary>
        /// Страна расположения банка.
        /// </summary>
        public CountryDataModel BankCountry { get; set; }

        // Так можно делать?
        public BankRatesDataModel()
        {
            BankCountry = new CountryDataModel();
        }
    }
}
