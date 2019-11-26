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

        /// <summary>
        /// Конвертирует значение единицы измерения валюты в значение, пригодное для парсинга в int.
        /// </summary>
        /// <param name="text">Строка для конвертации.</param>
        /// <returns>Строка, пригодная для парсинга в int.</returns>
        public delegate string ConvertedUnitHandler(string text);

        public ConvertedUnitHandler GetConvertedUnit { get; set; }
    }
}
