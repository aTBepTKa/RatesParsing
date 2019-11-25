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
        /// Название банка.
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Код валюты банка.
        /// </summary>
        public string BankCurrency { get; set; }

        /// <summary>
        /// Курсы валют банка.
        /// </summary>
        public IEnumerable<CurrencyDataModel> ExchangeRates { get; set; }
    }
}
