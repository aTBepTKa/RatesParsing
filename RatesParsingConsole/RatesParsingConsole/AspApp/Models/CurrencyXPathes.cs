﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.AspApp.Models
{
    // TODO: Выпилить из модели Dto после прикручивания JSON.
    /// <summary>
    /// Содержит XPath пути для валюты.
    /// </summary>
    public class CurrencyXPathes
    {
        /// <summary>
        /// Сокращенное название валюты.
        /// </summary>
        public string TextCode { get; set; }

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
