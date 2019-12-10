using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.AspApp.Models
{
    /// <summary>
    /// Содержит запросы к банкам.
    /// </summary>
    class BankRequest
    {
        /// <summary>
        /// ID запроса.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование банка.
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Текстовый код основной валюты банка.
        /// </summary>
        public string BankCurrency { get; set; }

        /// <summary>
        /// Ссылка на страницу с курсами.
        /// </summary>
        public string RatesUrlPage { get; set; }

        /// <summary>
        /// XPath пути для получения данных о валюте. Переделать в sting (JSON) и обрабатывать в коде используя рефлексию (шта?).
        /// </summary>
        public CurrencyXPathes XPathes { get; set; }

        /// <summary>
        /// Разделитель десятичной части числа.
        /// </summary>
        public string NumberDecimalSeparator { get; set; }

        /// <summary>
        /// Разделитель групп разрядов числа.
        /// </summary>
        public string NumberGroupSeparator { get; set; }

        /// <summary>
        /// Переменная часть адреса XPath.
        /// </summary>
        public string VariablePartOfXpath { get; set; }

        /// <summary>
        /// Начальная строка для считывания.
        /// </summary>
        public int StartXpathRow { get; set; }

        /// <summary>
        /// Последняя строка для считывания.
        /// </summary>
        public int EndXpathRow { get; set; }

        /// <summary>
        /// Команды для обработки строки единицы измерения Unit.
        /// </summary>
        public Dictionary<string, string[]> UnitScripts { get; set; }

        /// <summary>
        /// Команды для обработки строки текстового кода валюты TextCode.
        /// </summary>
        public IDictionary<string, string[]> TextCodeScripts { get; set; }
    }
}
