using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.Models
{
    /// <summary>
    /// Содержит данные о валюте.
    /// </summary>
    class CurrencyDataModel
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
        public int Unit { get; set; }

        /// <summary>
        /// Обменный курс валюты.
        /// </summary>
        public decimal ExchangeRate { get; set; }
    }
}
