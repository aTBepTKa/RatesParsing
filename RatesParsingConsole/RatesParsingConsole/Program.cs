using System;
using System.IO;
using HtmlAgilityPack;
using System.Collections.Generic;
using RatesParsingConsole.RateDataBase;

namespace RatesParsingConsole
{
    class Program
    {
        /// <summary>
        /// Получает курсы обмена валют с различных банков и сохраняет в базу (то что в будущем будет делать MVC приложение).
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Коллекция курсов по каждому банку (типа БД).
            List<BankRatesData> Banks = new List<BankRatesData>();

            // Список запросов к банкам.
            List<RequestBankData> requests = GetRequestBankDatas();

            // Перебрать список запросов и получить данные по валютам.
            foreach (var req in requests)
            {
                Banks.Add(GetBankRatesData(req));
            }
            // Вывести полученные значения для одного банка для одной валюты.
            foreach (var bank in Banks)
            {
                Console.WriteLine($"Название банка: {bank.BankCountry.BankName}.");
                Console.WriteLine();
                foreach (var currency in bank.CurrencyDatas)
                {
                    Console.WriteLine($"Краткое наименование валюты: {currency.ShortName}.");
                    Console.WriteLine($"Полное наименование валюты: {currency.FullName}.");
                    Console.WriteLine($"Обменный курс: {currency.ExchangeRate}.");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Получить список запросов к банкам (Часть MVC приложения).
        /// </summary>
        /// <returns></returns>
        static List<RequestBankData> GetRequestBankDatas()
        {
            // Сформировать запрос к одному банку.
            var requestBankData = new RequestBankData();

            // Установить адрес запрашиваемой страницы и название банка.
            requestBankData.RatesUrlPage = "https://www.nbg.gov.ge/index.php?m=582&lng=eng";
            requestBankData.BankCountry.BankName = "National Bank of Georgia";

            // Получить сценарий парсинга.
            requestBankData.XPathes = GetParsingScript();

            // Сформировать коллекцию запросов. Работаем пока с одним банком.
            var requestBankDataList = new List<RequestBankData>
            {
                requestBankData
            };

            return requestBankDataList;
        }

        /// <summary>
        /// Получить сценарий парсинга.
        /// </summary>
        /// <returns></returns>
        static CurrencyXPathes GetParsingScript()
        {
            var pathes = new CurrencyXPathes()
            {
                ShortName = @"//*[@id='currency_id']/table/tr[1]/td[1]",
                FullName = @"//*[@id='currency_id']/table/tr[1]/td[2]",
                Unit = "доработать",
                ExchangeRate = @"//*[@id='currency_id']/table/tr[1]/td[3]"
            };
            return pathes;
        }

        /// <summary>
        /// Получить данные курсов конкретного банка (Часть консольного приложения).
        /// </summary>
        /// <param name="requestBankData">Данные для получения курса</param>
        /// <returns></returns>
        static BankRatesData GetBankRatesData(RequestBankData requestBankData)
        {
            // Получить html страницу.
            var gettingHtml = new GettingHtml();
            HtmlDocument htmlDocument = gettingHtml.GetHtmlDocumentFromWeb(requestBankData.RatesUrlPage);

            // Хранилище данных валюты.
            var bankRatesData = new BankRatesData();
            bankRatesData.BankCountry.BankName = requestBankData.BankCountry.BankName;

            // Выполнить сценарий парсинга. Работаем пока с одной валютой.
            // Коллекция всех валют банка.
            bankRatesData.CurrencyDatas = new List<CurrencyData>();
            // Данные одной валюты банка.
            CurrencyData currencyData = GetCurrencyData(htmlDocument, requestBankData.XPathes);
            bankRatesData.CurrencyDatas.Add(currencyData);

            return bankRatesData;
        }

        /// <summary>
        /// Получить данные одной валюты.
        /// </summary>
        /// <param name="html">Страница для поиска валюты.</param>
        /// <param name="pathes">Адреса XPath.</param>
        /// <returns></returns>
        static CurrencyData GetCurrencyData(HtmlDocument html, CurrencyXPathes pathes)
        {
            var currencyData = new CurrencyData()
            {
                ShortName = GetValueByXPath(html, pathes.ShortName),
                FullName = GetValueByXPath(html, pathes.FullName),
                // Не работает конвертация, пока делаем ExchangeRate типа string.
                ExchangeRate = GetValueByXPath(html, pathes.ExchangeRate)
                // Еще должна быть единица измерения Unit. Рассмотреть отдельно, так как она сидит вместе с FullName.
            };
            return currencyData;
        }

        /// <summary>
        /// Получить значение по адресу XPath.
        /// </summary>
        /// <param name="html">Страница для парсинга.</param>
        /// <param name="xpath">Адрес XPath искомого значения.</param>
        /// <returns></returns>
        static string GetValueByXPath(HtmlDocument html, string xpath)
        {
            string result = html.DocumentNode.SelectSingleNode(xpath).InnerText;
            result = GetClearText(result);
            return result;
        }

        /// <summary>
        /// Получить текст без лишних пробелов, новых строк и т. п.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static string GetClearText(string text)
        {
            // Убрать новые строки и пробелы. С пробелами щээ подумать: необходимо их оставлять между словами.
            // Или вообще выпилить эту фишку нахуй либо в другое место, в GetParsingScript например.
            text = text.Replace("\n", "").Replace("\r", "").Replace(" ", "");
            return text;
        }

    }
}
