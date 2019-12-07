using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using RatesParsingConsole.AspApp.Models;
using System.Text;

namespace RatesParsingConsole.AspApp
{
    class RequestFactory
    {
        /// <summary>
        /// Получить список запросов к банкам из кода.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BankRequest> GetBankRequestsFromCode()
        {
            // TODO: получение данных из кода == получение данных из БД.

            // Список банков.
            var bankDataModels = new List<BankRequest>();

            // Установить данные для банков.
            // Переменная часть пути XPath для всех банков.
            var VariablePartOfXpath = "$VARIABLE";

            // Данные для Банка 1.
            // Данные для реализации цикла перебора строк с курсами валют.
            var StartRow1 = 1;
            // Последняя строка задана на 2 больше, чтобы проверить отработку ошибок (должно быть 43).
            var EndRow1 = 45;
            var bank1 = new BankRequest
            {
                BankName = "National Bank of Georgia",
                BankCurrency = "GEL",
                RatesUrlPage = "https://www.nbg.gov.ge/index.php?m=582&lng=eng",
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = ",",
                StartXpathRow = StartRow1,
                EndXpathRow = EndRow1,
                VariablePartOfXpath = VariablePartOfXpath
            };
            // Шаблон XPath пути для валюты.            
            bank1.XPathes = new CurrencyXPathes()
            {
                TextCode = "//*[@id='currency_id']/table/tr[$VARIABLE]/td[1]",
                Unit = "//*[@id='currency_id']/table/tr[$VARIABLE]/td[2]",
                ExchangeRate = "//*[@id='currency_id']/table/tr[$VARIABLE]/td[3]"
            };
            // Задаем команды для обработки текста.
            bank1.UnitScripts = new Dictionary<string, string[]>
            {
                {"GetNumberFromText", new string[0]}
            };
            bankDataModels.Add(bank1);


            // Данные для Банка 2.
            var StartRow2 = 2;
            var EndRow2 = 36;
            var bank2 = new BankRequest
            {
                BankName = "National Bank of Poland",
                BankCurrency = "PLN",
                RatesUrlPage = "https://www.nbp.pl/homen.aspx?f=/kursy/RatesA.html",
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = ",",
                StartXpathRow = StartRow2,
                EndXpathRow = EndRow2,
                VariablePartOfXpath = VariablePartOfXpath
            };
            bank2.XPathes = new CurrencyXPathes()
            {
                TextCode = @"//*[@id=""article""]/table/tr/td/center/table[1]/tr[$VARIABLE]/td[2]",
                Unit = @"//*[@id=""article""]/table/tr/td/center/table[1]/tr[$VARIABLE]/td[2]",
                ExchangeRate = @"//*[@id=""article""]/table/tr/td/center/table[1]/tr[$VARIABLE]/td[3]"
            };
            // Сформировать число из найденных в строке цифр.
            bank2.UnitScripts = new Dictionary<string, string[]>
            {
                {"GetNumberFromText", new string[0] }
            };
            bank2.TextCodeScripts = new Dictionary<string, string[]>
            {
                {"GetTextCodeFromEnd", new string[] {"3"} }
            };
            bankDataModels.Add(bank2);


            // Данные для банка 3.
            var StartRow3 = 2;
            var EndRow3 = 35;
            var bank3 = new BankRequest
            {
                BankName = "The Central Bank of the Russian Federation",
                BankCurrency = "RUB",
                RatesUrlPage = "https://www.cbr.ru/eng/currency_base/daily/",
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = ",",
                StartXpathRow = StartRow3,
                EndXpathRow = EndRow3,
                VariablePartOfXpath = VariablePartOfXpath
            };
            bank3.XPathes = new CurrencyXPathes()
            {
                TextCode = @"//*[@id='content']/table/tbody/tr[$VARIABLE]/td[2]",
                Unit = @"//*[@id='content']/table/tbody/tr[$VARIABLE]/td[3]",
                ExchangeRate = @"//*[@id='content']/table/tbody/tr[$VARIABLE]/td[5]"
            };
            bankDataModels.Add(bank3);


            // Данные для банка 4.
            var StartRow4 = 1;
            var EndRow4 = 32;
            var bank4 = new BankRequest
            {
                BankName = "European Central Bank",
                BankCurrency = "EUR",
                RatesUrlPage = "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html",
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = ",",
                StartXpathRow = StartRow4,
                EndXpathRow = EndRow4,
                VariablePartOfXpath = VariablePartOfXpath
            };
            bank4.XPathes = new CurrencyXPathes()
            {
                TextCode = @"//*[@id='ecb-content-col']/main/div/table/tbody/tr[$VARIABLE]/td[1]",
                Unit = @"//*[@id='ecb-content-col']/main/div/table/tbody/tr[$VARIABLE]/td[1]",
                ExchangeRate = @"//*[@id='ecb-content-col']/main/div/table/tbody/tr[$VARIABLE]/td[3]"
            };
            bank4.UnitScripts = new Dictionary<string, string[]>
            {
                {"GetFixedValue", new string[] { "1" } }
            };
            bankDataModels.Add(bank4);

            return bankDataModels;
        }

        /// <summary>
        /// Получить список запросов к банкам из .json файла
        /// </summary>
        /// <param name="fileName">Путь к файлу .json.</param>
        /// <returns></returns>
        public IEnumerable<BankRequest> GetBankRequestsFromFile(string fileName)
        {
            // TODO: обработать ошибки чтения .json файла.
            IEnumerable<BankRequest> requests;

            string JsonText;
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.UTF8))
                    JsonText = sr.ReadToEnd();
                requests = JsonSerializer.Deserialize<IEnumerable<BankRequest>>(JsonText);
                return requests;
            }
            else
            {
                return Array.Empty<BankRequest>();
            }
        }
    }
}
