using HtmlAgilityPack;
using RatesParsingConsole.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RatesParsingConsole
{
    /// <summary>
    /// Представляет средства для парсинга страницы банка.
    /// </summary>
    class ExchangeRatesFactory
    {
        /// <summary>
        /// Получить списки курсов валют по банкам.
        /// </summary>
        /// <param name="bankDataRequestDtos">Список запросов для банков.</param>
        /// <returns></returns>
        public IEnumerable<BankRatesDataModel> GetBankRatesDatas(IEnumerable<BankDataRequestDto> requests)
        {
            // Коллекция банков.
            var banks = new List<BankRatesDataModel>();

            // Перебрать запросы по банкам и получить курсы валют.
            foreach (var req in requests)
            {
                BankRatesDataModel ratesData = GetBankRatesData(req);
                if (ratesData == null)
                    Console.WriteLine($"Ошибка при загрузке данных из банка {req.BankCountry.BankName}.");
                else
                    banks.Add(ratesData);
            }
            return banks;
        }

        /// <summary>
        /// Получить данные курсов конкретного банка.
        /// </summary>
        /// <param name="bankDataRequest">Данные для получения курса</param>
        /// <returns></returns>
        private BankRatesDataModel GetBankRatesData(BankDataRequestDto bankDataRequest)
        {
            // Получить html страницу.
            var gettingHtml = new GettingHtml();
            HtmlDocument htmlDocument = gettingHtml.GetHtmlDocumentFromWeb(bankDataRequest.RatesUrlPage);

            // Прервать выполнение программы, если возникла ошибка при загрузке страницы.
            if (htmlDocument == null)
                return null;

            // Хранилище данных валюты.
            var bankRatesData = new BankRatesDataModel();
           
            bankRatesData.BankCountry.BankName = bankDataRequest.BankCountry.BankName;

            // Выполнить сценарий парсинга. Работаем пока с одной валютой.
            // Получить данные одной валюты банка.
            CurrencyDataModel currencyData = GetCurrencyData(htmlDocument, bankDataRequest);

            var currencyDatas = new List<CurrencyDataModel>
            {
                currencyData
            };
            bankRatesData.CurrencyDatas = currencyDatas;

            return bankRatesData;
        }

        /// <summary>
        /// Получить данные одной валюты.
        /// </summary>
        /// <param name="html">Страница для поиска валюты.</param>
        /// <param name="pathes">Адреса XPath.</param>
        /// <returns></returns>
        private CurrencyDataModel GetCurrencyData(HtmlDocument html, BankDataRequestDto requestDto)
        {
            // Установить адреса XPath.
            CurrencyXPathesDto pathes = requestDto.XPathes;

            // Получить текстовую часть данных валюты (наименование).
            var currencyData = new CurrencyDataModel()
            {
                ShortName = GetValueByXPath(html, pathes.ShortName),
                FullName = GetValueByXPath(html, pathes.FullName),
            };

            // Получить численную часть данных валюты.
            // Установить разделитель разрядов и дроби.
            var formatInfo = new NumberFormatInfo
            {
                NumberDecimalSeparator = requestDto.NumberDecimalSeparator,
                NumberGroupSeparator = requestDto.NumberGroupSeparator
            };

            // Получить числовые значения в виде строки.
            string exchangeRate = GetValueByXPath(html, pathes.ExchangeRate);
            string unit = GetValueByXPath(html, pathes.Unit);

            // Конвертировать строку в числа. 
            // ДОБАВИТЬ ОБРАБОТКУ ОШИБОК КОНВЕРТАЦИИ.
            currencyData.ExchangeRate = decimal.Parse(exchangeRate, formatInfo);
            // currencyData.Unit = int.Parse(unit, formatInfo);

            return currencyData;
        }

        /// <summary>
        /// Получить значение по адресу XPath.
        /// </summary>
        /// <param name="html">Страница для парсинга.</param>
        /// <param name="xpath">Адрес XPath искомого значения.</param>
        /// <returns></returns>
        private string GetValueByXPath(HtmlDocument html, string xpath)
        {
            // Узел целевого значения.
            HtmlNode resultNode;
            // Целевое значение.
            string result;

            // Попытка получить узел.
            try
            {
                resultNode = html.DocumentNode.SelectSingleNode(xpath);
            }
            catch (System.Xml.XPath.XPathException)
            {
                Console.WriteLine("Ошибка при обработке XPath адреса");
                resultNode = null;
            }

            if (resultNode != null)
                result = resultNode.InnerText;
            else
                result = "ERROR";
            // Привести форматирование текста к приемлемому виду.
            result = GetClearText(result);
            return result;
        }

        /// <summary>
        /// Получить текст без лишних пробелов, новых строк и т. п.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetClearText(string text)
        {
            // Убрать новые строки и пробелы. С пробелами щээ подумать: необходимо их оставлять между словами.
            // Или вообще выпилить эту фишку нахуй либо в другое место, в GetParsingScript например.
            text = text.Replace("\n", "").Replace("\r", "").Replace(" ", "");
            return text;
        }
    }
}
