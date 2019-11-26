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
        public string TextCode { get; set; }

        /// <summary>
        /// Единица измерения валюты.
        /// </summary>
        public int Unit { get; set; }

        /// <summary>
        /// Обменный курс валюты.
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Успешно ли получены значения валюты.
        /// </summary>
        public bool IsSuccessfullyParsed { get; set; }

        /// <summary>
        /// Описание ошибки при неудачном получении данных валюты.
        /// </summary>
        public string ErrorName { get; set; }
    }
}
