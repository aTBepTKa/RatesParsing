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
        public CountryData BankCountry { get; set; }

        /// <summary>
        /// Ссылка на страницу с курсами.
        /// </summary>
        public string RatesUrlPage { get; set; }

        /// <summary>
        /// XPath пути для получения данных о валюте.
        /// </summary>
        public CurrencyXPathes XPathes { get; set; }

        public RequestBankData()
        {
            BankCountry = new CountryData();
        }
    }
}
