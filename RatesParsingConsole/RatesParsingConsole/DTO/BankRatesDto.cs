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
        /// SWIFT код банка.
        /// </summary>
        public string SwiftCode { get; set; }

        /// <summary>
        /// Название банка.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код валюты банка.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Дата выполнения парсинга.
        /// </summary>
        public DateTime DateStamp { get; set; }

        /// <summary>
        /// Курсы валют банка.
        /// </summary>
        public IEnumerable<CurrencyDataDto> ExchangeRates { get; set; }
    }
}
