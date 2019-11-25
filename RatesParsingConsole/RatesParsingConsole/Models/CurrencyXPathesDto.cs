using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.Models
{
    // TODO: Выпилить из модели Dto после прикручивания JSON.
    /// <summary>
    /// Содержит XPath пути для валюты.
    /// </summary>
    class CurrencyXPathesDto
    {
        /// <summary>
        /// Сокращенное название валюты.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Полное название валюты.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Единица измерения валюты.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Обменный курс валюты.
        /// </summary>
        public string ExchangeRate { get; set; }
    }
}
