using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.Models
{
    /// <summary>
    /// Данные курсов валют банка.
    /// </summary>
    class BankRatesModel
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

        /// <summary>
        /// Успешно ли получены значения валюты.
        /// </summary>
        public bool IsSuccessfullyParsed { get; set; }

        /// <summary>
        /// Описание ошибки при неудачном получении данных валюты.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
