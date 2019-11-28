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
            // Получить html страницу.
            var gettingHtml = new GettingHtml();
            HtmlDocument htmlDocument = gettingHtml.GetHtmlDocumentFromWeb(request.RatesUrlPage);

            // Прервать выполнение, если страница не получена.
            if (htmlDocument == null)
                return Array.Empty<CurrencyDataModel>();

            // Хранилище данных валюты.
            var currencyDataList = new List<CurrencyDataModel>();

            // Выполнить парсинг валют банка.
            for (var i = request.StartRow; i <= request.RowsNum; i++)
            {
                CurrencyDataModel currencyData = GetCurrencyData(htmlDocument, request, i);
                currencyDataList.Add(currencyData);
            }
            return currencyDataList;
        }

        /// <summary>
        /// Получить данные одной валюты.
        /// </summary>
        /// <param name="html">Страница для поиска валюты.</param>
        /// <param name="pathes">Адреса XPath.</param>
        /// <returns></returns>
        private CurrencyDataModel GetCurrencyData(HtmlDocument html, BankDataRequestDto request, int rowNum)
        {
            // Данные валюты.
            var currencyData = new CurrencyDataModel();

            // Установить переменную часть XPath адреса.
            var pathes = new CurrencyXPathesDto();
            try
            {
                pathes = GetActualXpath(request.XPathes, request.VariablePartOfXpath, rowNum.ToString());
            }
            catch (Exception e)
            {
                currencyData.IsSuccessfullyParsed = false;
                currencyData.ErrorName = $"Ошибка при получении уточненного адреса XPath: {e.Message}";
                return currencyData;
            }


            // Установить разделитель разрядов и дроби.
            var formatInfo = new NumberFormatInfo()
            {
                NumberDecimalSeparator = request.NumberDecimalSeparator,
                NumberGroupSeparator = request.NumberGroupSeparator
            };

            // Получить значения данных для валюты со страницы через XPath.
            try
            {
                // Получить значения по адресу XPath
                string textCode = GetValueByXPath(html, pathes.TextCode);                
                string exchangeRate = GetValueByXPath(html, pathes.ExchangeRate);
                string unit = GetValueByXPath(html, pathes.Unit);

                // Выделить из строки необходимый текст.
                unit = request.GetUnitSubString(unit);
                textCode = request.GetTextCodeSubString(textCode);

                // 
                currencyData.TextCode = textCode;
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

        /// <summary>
        /// Получить 
        /// </summary>
        /// <param name="xPathesDto">XPath адрес.</param>
        /// <param name="OldSubString">Переменная часть XPath адреса</param>
        /// <param name="NewSubString">Значение переменной части XPath адреса</param>
        /// <returns></returns>
        private CurrencyXPathesDto GetActualXpath(CurrencyXPathesDto xPathesDto, string OldSubString, string NewSubString)
        {
            var NewXpathes = new CurrencyXPathesDto();
            NewXpathes.ExchangeRate = xPathesDto.ExchangeRate.Replace(OldSubString, NewSubString);
            NewXpathes.TextCode = xPathesDto.TextCode.Replace(OldSubString, NewSubString);
            NewXpathes.Unit = xPathesDto.Unit.Replace(OldSubString, NewSubString);
            return NewXpathes;
        }
    }
}
