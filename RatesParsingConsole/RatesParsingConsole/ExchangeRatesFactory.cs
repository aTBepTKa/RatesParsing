using HtmlAgilityPack;
using RatesParsingConsole.Models;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Text;

namespace RatesParsingConsole
{
    /// <summary>
    /// Представляет средства для парсинга страницы банка.
    /// </summary>
    class ExchangeRatesFactory
    {

        /// <summary>
        /// Получить курсы валют банка асинхронно.
        /// </summary>
        /// <param name="request">Данные для запроса банку.</param>
        /// <param name="sleep">(Временно). Время приостановки потока для проверки асинхронности.</param>
        /// <returns></returns>
        public async Task<BankRatesModel> GetCurrencyDatasAsync(BankRequestDto request, int sleep = 0)
        {
            Thread.Sleep(sleep);
            var result = await Task.Run(() => GetCurrencyDatas(request));
            return result;
        }

        /// <summary>
        /// Получить курсы валют банка.
        /// </summary>
        /// <param name="request">Данные для запроса банку.</param>
        /// <returns></returns>
        public BankRatesModel GetCurrencyDatas(BankRequestDto request)
        {
            var BankRates = new BankRatesModel
            {
                BankName = request.BankName,
                BankCurrency = request.BankCurrency,
                RequestResultStatus = ProcessingResultModel.ProcessingResult.Success
            };

            var gettingHtml = new GettingHtml();
            HtmlDocument htmlDocument = gettingHtml.GetHtmlFromWeb(request.RatesUrlPage);

            List<CurrencyDataModel> currencyDataList;

            // Выполнить парсинг валют банка по строкам.
            if (htmlDocument != null)
            {
                currencyDataList = new List<CurrencyDataModel>(request.EndRow - request.StartRow + 1);
                for (var i = request.StartRow; i <= request.EndRow; i++)
                {
                    CurrencyDataModel currencyData = GetCurrencyData(htmlDocument, request, i);
                    // Проверить успешность получения данных валюты. Тут что нибудь с енумом будет.
                    if (currencyData.RequestResultStatus != ProcessingResultModel.ProcessingResult.Success)
                    {
                        BankRates.RequestResultStatus = ProcessingResultModel.ProcessingResult.ProcessedWithErrors;
                        BankRates.RequestResultMessage += $"Ошибка при получении данных валюты {currencyData.TextCode}. ";
                    }
                    currencyDataList.Add(currencyData);
                }
                BankRates.ExchangeRates = currencyDataList;
            }
            else
            {
                BankRates.RequestResultStatus = ProcessingResultModel.ProcessingResult.Error;
                BankRates.RequestResultMessage = "Ошибка при получении html страницы.";
                BankRates.ExchangeRates = Array.Empty<CurrencyDataModel>();
            }
            return BankRates;
        }

        /// <summary>
        /// Получить данные одной валюты.
        /// </summary>
        /// <param name="html">Страница для поиска валюты.</param>
        /// <param name="pathes">Адреса XPath.</param>
        /// <returns></returns>
        private CurrencyDataModel GetCurrencyData(HtmlDocument html, BankRequestDto request, int rowNum)
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
                currencyData.RequestResultStatus = ProcessingResultModel.ProcessingResult.Error;
                currencyData.RequestResultMessage = $"Ошибка при получении уточненного адреса XPath: {e.Message}";
                return currencyData;
            }


            // Установить разделитель разрядов и дроби.
            var formatInfo = new NumberFormatInfo
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

                // Конвертация строки в число (обменный курс).
                currencyData.RequestResultStatus = ProcessingResultModel.ProcessingResult.Success;
                if (decimal.TryParse(exchangeRate, NumberStyles.Currency, formatInfo, out decimal exchangeRateResult))
                    currencyData.ExchangeRate = exchangeRateResult;
                else
                {
                    currencyData.ExchangeRate = 0;
                    currencyData.RequestResultStatus = ProcessingResultModel.ProcessingResult.Error;
                    currencyData.RequestResultMessage += "Ошибка при конвертации зачения обменного курса (ExchangeRate). ";
                }

                // Конвертация строки в число (единица измерения).
                if (int.TryParse(unit, out int unitResult))
                    currencyData.Unit = unitResult;
                else
                {
                    currencyData.Unit = 0;
                    currencyData.RequestResultStatus = ProcessingResultModel.ProcessingResult.Error;
                    currencyData.RequestResultMessage += "Ошибка при конвертации зачения единицы измерения валюты (Unit). ";
                }
                currencyData.ExchangeRate = decimal.Parse(exchangeRate, formatInfo);
                currencyData.Unit = int.Parse(unit, formatInfo);

                currencyData.TextCode = textCode;
            }
            catch (Exception e)
            {
                currencyData.RequestResultStatus = ProcessingResultModel.ProcessingResult.Error;
                currencyData.RequestResultMessage = e.Message;
            }
            return currencyData;
        }

        /// <summary>
        /// Получить значение по адресу XPath.
        /// </summary>
        /// <param name="html">Страница для парсинга.</param>
        /// <param name="xpath">Адрес XPath искомого значения.</param>
        /// <returns></returns>
        private static string GetValueByXPath(HtmlDocument html, string xpath)
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
        private static string GetClearText(string text)
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
            var NewXpathes = new CurrencyXPathesDto
            {
                ExchangeRate = xPathesDto.ExchangeRate.Replace(OldSubString, NewSubString),
                TextCode = xPathesDto.TextCode.Replace(OldSubString, NewSubString),
                Unit = xPathesDto.Unit.Replace(OldSubString, NewSubString)
            };
            return NewXpathes;
        }
    }
}
