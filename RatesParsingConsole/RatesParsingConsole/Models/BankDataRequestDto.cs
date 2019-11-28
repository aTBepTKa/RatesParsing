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
        /// Переменная часть адреса XPath.
        /// </summary>
        public string VariablePartOfXpath { get; set; }

        /// <summary>
        /// Начальная строка для считывания.
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// Количество строк для считывания.
        /// </summary>
        public int RowsNum { get; set; }

        /// <summary>
        /// Получить подстроку с необходимыми данными.
        /// </summary>
        /// <param name="text">Строка для конвертации.</param>
        /// <returns></returns>
        public delegate string GetSubString(string text);

        /// <summary>
        /// Получить строку единицы измерения валюты в формате для парсинга.
        /// </summary>
        public GetSubString GetUnitSubString { get; set; } = delegate (string text)
        {
            return text;
        };

        /// <summary>
        /// Получить текстовый код валюты.
        /// </summary>
        public GetSubString GetTextCodeSubString { get; set; } = delegate (string text)
        {
            return text;
        };

    }
}
