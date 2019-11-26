using System;
using System.Collections.Generic;
using RatesParsingConsole.Models;

namespace RatesParsingConsole
{
    class Program
    {
        //Устранены не все замечания. Пересмотреть еще раз.

        /// <summary>
        /// Получает курсы обмена валют с различных банков и выводит в консоль.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Формируем список банков с запросами.
            var bankDataModelList = new List<BankDataModel>();
            bankDataModelList.AddRange(GetBankData());

            // Инструмент для получения результатов парсинга страниц.
            var ratesFactory = new ExchangeRatesFactory();
            
            // Получить курсы валют.
            foreach (var bank in bankDataModelList)            
                bank.BankRates.ExchangeRates = ratesFactory.GetBankRatesDatas(bank.BankDataRequest);

            // Отделить данные запросов от данных курсов.
            var bankRatesList = new List<BankRatesDataModel>();
            foreach (var bank in bankDataModelList)            
                bankRatesList.Add(bank.BankRates);            

            // Вывести полученные значения для одного банка для одной валюты. Будем пилить дальше.
            ShowResults(bankRatesList);
            Console.ReadKey();
        }

        /// <summary>
        /// Сформировать список банков с данными запроса.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<BankDataModel> GetBankData()
        {
            // Список банков.
            var bankDataModels = new List<BankDataModel>();

            // Установить данные для банков.
            // TODO: Разработать БД с информацией по банкам и соответствующими запросами.
            var bank1 = new BankDataModel();
            bank1.BankRates.BankName = "National Bank of Georgia";
            bank1.BankRates.BankCurrency = "GEL";
            bank1.BankDataRequest.RatesUrlPage = "https://www.nbg.gov.ge/index.php?m=582&lng=eng";
            bank1.BankDataRequest.NumberDecimalSeparator = ".";
            bank1.BankDataRequest.NumberGroupSeparator = ",";
            // XPath пути для валюты.
            // TODO: реализовать работу с данными JSON.
            bank1.BankDataRequest.XPathes = new CurrencyXPathesDto()
            {
                TextCode = @"//*[@id='currency_id']/table/tr[1]/td[1]",
                Unit = @"//*[@id='currency_id']/table/tr[1]/td[2]",
                ExchangeRate = @"//*[@id='currency_id']/table/tr[1]/td[3]"
            };
            // Получить число (единицу измерения валюты) из текстовой строки.
            bank1.BankDataRequest.GetConvertedUnit = delegate (string text)
            {
                string digitText = "";

                foreach (char ch in text)
                {
                    if (char.IsDigit(ch))
                        digitText += ch;
                }

                return digitText;
            };

            // Добавить банк в список.
            bankDataModels.Add(bank1);
            
            var bank2 = new BankDataModel();
            bank2.BankRates.BankName = "National Bank of Poland";
            bank2.BankRates.BankCurrency = "PLN";
            bank2.BankDataRequest.RatesUrlPage = "http://www.nbp.pl/homen.aspx?f=/kursy/ratesa.html";
            bank2.BankDataRequest.NumberDecimalSeparator = ".";
            bank2.BankDataRequest.NumberGroupSeparator = ",";
            bankDataModels.Add(bank2);
            bank2.BankDataRequest.XPathes = new CurrencyXPathesDto();

            return bankDataModels;
        }

        /// <summary>
        /// Выводит полученные результаты в консоль.
        /// </summary>
        /// <param name="banks"></param>
        static void ShowResults(IEnumerable<BankRatesDataModel> banks)
        {
            Console.WriteLine("\nКурсы валют по банкам:\n");
            foreach (var bank in banks)
            {
                Console.WriteLine($"Курсы валют банка \"{bank.BankName}\", " +
                    $"национальная валюта {bank.BankCurrency}:");
                Console.WriteLine();
                foreach (var currency in bank.ExchangeRates)
                {
                    if (currency.IsSuccessfullyParsed)
                    {
                        Console.WriteLine($"Краткое наименование валюты: {currency.TextCode}.");
                        Console.WriteLine($"Единица измерения валюты: {currency.Unit}.");
                        Console.WriteLine($"Обменный курс: {currency.ExchangeRate}.");
                        Console.WriteLine();
                    }
                    else
                        Console.WriteLine($"Ошибка при получении данных валюты: {currency.ErrorName}.");
                }
                Console.WriteLine();
            }
        }
    }
}
