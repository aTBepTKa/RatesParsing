using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatesParsingConsole.DTO.Json
{
    /// <summary>
    /// Представляет данные банка с данными для выполнения парсинга страниц и результатами парсинга.
    /// </summary>
    public class BankJson
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
        /// Обменные курсы банка (результаты парсинга).
        /// </summary>
        public virtual IEnumerable<ExchangeRateListJson> ExchangeRateLists { get; set; }
    }
}
