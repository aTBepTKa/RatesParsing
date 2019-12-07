using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.AspApp.Models
{
    /// <summary>
    /// Данные курсов валют банка.
    /// </summary>
    class BankRates : ProcessingResult
    {
        /// <summary>
        /// ID банка.
        /// </summary>
        public int ID { get; set; }

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
        public IEnumerable<CurrencyData> ExchangeRates { get; set; }

    }
}
