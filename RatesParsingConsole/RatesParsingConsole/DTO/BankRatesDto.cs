using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.DTO
{
    /// <summary>
    /// Данные курсов валют банка.
    /// </summary>
    public class BankRatesDto : ProcessingResultDto
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
        public IEnumerable<CurrencyDataDto> ExchangeRates { get; set; }
    }
}
