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
        /// Получить курсы валют банка.
        /// </summary>
        /// <param name="request">Данные для запроса банку.</param>
        /// <returns></returns>
        public IEnumerable<CurrencyDataModel> GetBankRatesDatas(BankDataRequestDto request)
        {
            // Получить курсы валют.
            IEnumerable<CurrencyDataModel> currencyDatas = GetExchangeRatesData(request);
            if (currencyDatas == null)
                Console.WriteLine($"Ошибка при загрузке данных из {request.RatesUrlPage}.");
            return currencyDatas;
        }

        /// <summary>
        /// Получить данные курсов банка.
        /// </summary>
        /// <param name="request">Данные для получения курса</param>
        /// <returns></returns>
        private IEnumerable<CurrencyDataModel> GetExchangeRatesData(BankDataRequestDto request)
        {
            // Получить html страницу.
            var gettingHtml = new GettingHtml();
            HtmlDocument htmlDocument = gettingHtml.GetHtmlDocumentFromWeb(request.RatesUrlPage);

            // Прервать выполнение программы, если возникла ошибка при загрузке страницы.
            if (htmlDocument == null)
                return Array.Empty<CurrencyDataModel>();

            // Хранилище данных валюты.
            var currencyDataList = new List<CurrencyDataModel>();

            // Выполнить сценарий парсинга. Работаем пока с одной валютой.
            // TODO: реализовать получение списка обменных курсов.
            CurrencyDataModel currencyData = GetCurrencyData(htmlDocument, request);
            currencyDataList.Add(currencyData);

            return currencyDataList;
        }

        /// <summary>
        /// Получить данные одной валюты.
        /// </summary>
        /// <param name="html">Страница для поиска валюты.</param>
        /// <param name="pathes">Адреса XPath.</param>
        /// <returns></returns>
        private CurrencyDataModel GetCurrencyData(HtmlDocument html, BankDataRequestDto request)
        {
            // Установить адреса XPath.
            CurrencyXPathesDto pathes = request.XPathes;

            // Получить численную часть данных валюты.
            // Установить разделитель разрядов и дроби.
            var formatInfo = new NumberFormatInfo()
            {
                NumberDecimalSeparator = request.NumberDecimalSeparator,
                NumberGroupSeparator = request.NumberGroupSeparator
            };

            // Получить данные валюты через XPath.
            var currencyData = new CurrencyDataModel();

            

            // Получить значения данных для валюты со страницы.
            try
            {
                currencyData.TextCode = GetValueByXPath(html, pathes.TextCode);
                
                // Получить числовые значения в виде строки.
                string exchangeRate = GetValueByXPath(html, pathes.ExchangeRate);
                string unit = GetValueByXPath(html, pathes.Unit);

                // Отделить числовую часть строки от текстовой.
                unit = request.GetConvertedUnit(unit);

                // Конвертировать строку в числовые значения.
                currencyData.ExchangeRate = decimal.Parse(exchangeRate, formatInfo);
                currencyData.Unit = int.Parse(unit, formatInfo);
                currencyData.IsSuccessfullyParsed = true;
            }
            catch (Exception e)
            {
                currencyData.IsSuccessfullyParsed = false;
                currencyData.ErrorName = e.Message;
            }
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
                throw new Exception($"Ошибка при обработке XPath адреса {xpath}. ");
            }
            catch (ArgumentNullException)
            {
                throw new Exception("Отсутствует XPath адрес");
            }

            if (resultNode != null)
            {
                result = resultNode.InnerText;
                // Привести форматирование текста к приемлемому виду.
                result = GetClearText(result);
            }
            else
                throw new Exception("При поиске по адресу XPath получено значение Null");
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
