using System;
using System.Collections.Generic;
using System.Text;
using RatesParsingConsole.RateDataBase;

namespace RatesParsingConsole
{
    /// <summary>
    /// Данные для запроса к банку.
    /// </summary>
    class RequestBankData
    {
        /// <summary>
        /// Страна банка.
        /// </summary>
        public CountryData CountryData { get; set; }

        /// <summary>
        /// Ссылка на страницу с курсами.
        /// </summary>
        public string RatesUrlPage { get; set; }

        /// <summary>
        /// Сценарий парсинга страницы.
        /// </summary>
        public object ParsingScript { get; set; }
    }
}
