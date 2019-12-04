using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.Models
{
    /// <summary>
    /// Содержит данные о валюте.
    /// </summary>
    class CurrencyDataModel : ProcessingResultModel
    {
        /// <summary>
        /// Сокращенное название валюты.
        /// </summary>
        public string TextCode { get; set; }

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
