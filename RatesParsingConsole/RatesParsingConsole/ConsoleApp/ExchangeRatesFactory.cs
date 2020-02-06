using HtmlAgilityPack;
using RatesParsingConsole.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace RatesParsingConsole.ConsoleApp
{
    /// <summary>
    /// Представляет средства для парсинга страницы банка.
    /// </summary>
    class ExchangeRatesFactory
    {

        /// <summary>
        /// Получить курсы валют банка асинхронно.
        /// </summary>
        /// <param name="request">Данные для запроса к банку.</param>
        /// <returns></returns>
        public async Task<BankRatesDto> GetBankRatesAsync(BankRequestDto request)
        {
            var gettingHtml = new GettingHtml();
            HtmlDocument htmlDocument = await gettingHtml.GetHtmlFromWebAsync(request.RatesUrlPage);

            var bankRates = GetBankRatesFromHtml(htmlDocument, request);
            bankRates.DateStamp = DateTime.Now;
            return bankRates;
        }

        /// <summary>
        /// Получить курсы валют банка.
        /// </summary>
        /// <param name="request">Данные для запроса банку.</param>
        /// <returns></returns>
        public BankRatesDto GetBankRates(BankRequestDto request)
        {
            var gettingHtml = new GettingHtml();
            HtmlDocument htmlDocument = gettingHtml.GetHtmlFromWeb(request.RatesUrlPage);

            var bankRates = GetBankRatesFromHtml(htmlDocument, request);
            return bankRates;
        }

        /// <summary>
        /// Получить курсы валют банка по заданной html странице.
        /// </summary>
        /// <param name="html">Страница с обменными курсами.</param>
        /// <param name="request">Данные для запроса к банку.</param>
        /// <returns></returns>
        private BankRatesDto GetBankRatesFromHtml(HtmlDocument html, BankRequestDto request)
        {
            // Получить методы для обработки текста.
            WordProcessingHandler UnitProcess = GetMethods(request.UnitScripts);
            WordProcessingHandler TextCodeProcess = GetMethods(request.TextCodeScripts);
            var bankRates = new BankRatesDto
            {
                Name = request.Name,
                Currency = request.Currency,
                SwiftCode = request.SwiftCode,
                RequestResultStatus = ProcessingResultDto.ResultType.Success
            };
            List<CurrencyDataDto> currencyDataList;

            // Выполнить парсинг валют банка по строкам.
            if (html != null)
            {
                currencyDataList = new List<CurrencyDataDto>(request.EndXpathRow - request.StartXpathRow + 1);
                for (var i = request.StartXpathRow; i <= request.EndXpathRow; i++)
                {
                    CurrencyDataDto currencyData = GetCurrencyData(html, request, i, UnitProcess, TextCodeProcess);
                    // Проверить успешность получения данных валюты.
                    if (currencyData.RequestResultStatus != ProcessingResultDto.ResultType.Success)
                    {
                        bankRates.RequestResultStatus = ProcessingResultDto.ResultType.ProcessedWithErrors;
                        bankRates.RequestResultMessage += $"Ошибка при получении данных валюты. {currencyData.TextCode}{Environment.NewLine}";
                    }
                    currencyDataList.Add(currencyData);
                }
                bankRates.ExchangeRates = currencyDataList;
            }
            else
            {
                bankRates.RequestResultStatus = ProcessingResultDto.ResultType.Error;
                bankRates.RequestResultMessage = "Ошибка при получении html страницы.";
                bankRates.ExchangeRates = Array.Empty<CurrencyDataDto>();
            }
            return bankRates;
        }

        /// <summary>
        /// Получить методы из словаря.
        /// </summary>
        /// <param name="keys">Наименование методов и соответствующие параметры.</param>
        /// <returns></returns>
        private WordProcessingHandler GetMethods(IDictionary<string, string[]> keys)
        {
            if (keys != null)
            {
                WordProcessingHandler methods = null;
                // Получить тип объекта, содержащего методы.
                Type scriptsType = typeof(ScriptCommands);
                // Создать объект, содержащий методы.
                object scriptsObject = Activator.CreateInstance(scriptsType);
                // Получить список методов.
                foreach (var key in keys)
                {
                    // Получить метод по заданному имени из словаря.
                    MethodInfo method = scriptsType.GetMethod(key.Key);
                    var newMethod = method.Invoke(scriptsObject, key.Value);
                    methods += newMethod as WordProcessingHandler;
                }
                return methods;
            }
            else
                return text => text;
        }

        /// <summary>
        /// Получить данные одной валюты.
        /// </summary>
        /// <param name="html">Страница для поиска валюты.</param>
        /// <param name="pathes">Адреса XPath.</param>
        /// <returns></returns>
        private CurrencyDataDto GetCurrencyData(HtmlDocument html,
                                                BankRequestDto request,
                                                int rowNum,
                                                WordProcessingHandler unitProcess,
                                                WordProcessingHandler textcodeProcess)
        {
            // Данные валюты.
            var currencyData = new CurrencyDataDto();

            // Установить переменную часть XPath адреса.
            var pathes = new CurrencyXPathesDto();
            try
            {
                pathes = GetActualXpath(request.XPathes, request.VariablePartOfXpath, rowNum.ToString());
            }
            catch (Exception e)
            {
                currencyData.RequestResultStatus = ProcessingResultDto.ResultType.Error;
                currencyData.RequestResultMessage = $"Ошибка при получении уточненного адреса XPath: {e.Message}";
                return currencyData;
            }


            // Установить разделитель разрядов и десятичной части.
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
                unit = unitProcess(unit);
                textCode = textcodeProcess(textCode);

                // Конвертация строки в число (обменный курс).
                currencyData.RequestResultStatus = ProcessingResultDto.ResultType.Success;
                if (decimal.TryParse(exchangeRate, NumberStyles.Currency, formatInfo, out decimal exchangeRateResult))
                    currencyData.ExchangeRateValue = exchangeRateResult;
                else
                {
                    currencyData.ExchangeRateValue = 0;
                    currencyData.RequestResultStatus = ProcessingResultDto.ResultType.Error;
                    currencyData.RequestResultMessage += "Ошибка при конвертации зачения обменного курса (ExchangeRate). ";
                }

                // Конвертация строки в число (единица измерения).
                if (int.TryParse(unit, out int unitResult))
                    currencyData.Unit = unitResult;
                else
                {
                    currencyData.Unit = 0;
                    currencyData.RequestResultStatus = ProcessingResultDto.ResultType.Error;
                    currencyData.RequestResultMessage += "Ошибка при конвертации зачения единицы измерения валюты (Unit). ";
                }

                currencyData.TextCode = textCode;
            }
            catch (FormatException e)
            {
                currencyData.RequestResultStatus = ProcessingResultDto.ResultType.Error;
                currencyData.RequestResultMessage = e.Message;
            }
            catch (ArgumentNullException e)
            {
                currencyData.RequestResultStatus = ProcessingResultDto.ResultType.Error;
                currencyData.RequestResultMessage = e.ParamName;
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
                throw new System.Xml.XPath.XPathException($"Ошибка при обработке XPath адреса {xpath}. ");
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("Отсутствует XPath адрес");
            }

            if (resultNode != null)
            {
                result = resultNode.InnerText;
                // Привести форматирование текста к приемлемому виду.
                result = GetClearText(result);
            }
            else
                throw new ArgumentNullException("При поиске по адресу XPath получено значение Null");
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
        /// Получить кокнертный XPath путем замены переменной части на конкретное значение.
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
