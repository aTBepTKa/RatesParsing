using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.RateDataBase
{
    /// <summary>
    /// Содержит данные о валюте.
    /// </summary>
    class CurrencyData
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
        public double ExchangeRate { get; set; }
    }
}
