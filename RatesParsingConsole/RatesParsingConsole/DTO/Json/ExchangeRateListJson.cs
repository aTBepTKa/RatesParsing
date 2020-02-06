using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatesParsingConsole.DTO.Json
{
    /// <summary>
    /// Список обменных курсов банка.
    /// </summary>
    public class ExchangeRateListJson
    {
        /// <summary>
        /// Дата и время получения данных об обменных курсах.
        /// </summary>
        public DateTime DateTimeStamp { get; set; }

        /// <summary>
        /// Список обменных курсов.
        /// </summary>
        public virtual IEnumerable<ExchangeRateJson> ExchangeRates { get; set; }

    }
}
