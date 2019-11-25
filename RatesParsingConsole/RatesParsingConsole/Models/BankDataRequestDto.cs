using System;
using System.Collections.Generic;
using System.Text;
using RatesParsingConsole.Models;

namespace RatesParsingConsole.Models
{
    /// <summary>
    /// Данные для запроса к банку.
    /// </summary>
    class BankDataRequestDto
    {
        /// <summary>
        /// Ссылка на страницу с курсами.
        /// </summary>
        public string RatesUrlPage { get; set; }

        /// <summary>
        /// XPath пути для получения данных о валюте. Переделать в sting (JSON) и обрабатывать в коде используя рефлексию (шта?).
        /// </summary>
        public CurrencyXPathesDto XPathes { get; set; }

        /// <summary>
        /// Разделитель целой и дробной части.
        /// </summary>
        public string NumberDecimalSeparator { get; set; }

        /// <summary>
        /// Разделитель десятичной части числа.
        /// </summary>
        public string NumberGroupSeparator { get; set; }
    }
}
