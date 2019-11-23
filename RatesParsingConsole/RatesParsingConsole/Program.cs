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
            // Получить список запросов к банкам.
            var requestFactory = new RequestFactory();
            IEnumerable<BankDataRequestDto> requests = requestFactory.GetRequestBankDatas();

            // Получить данные по валютам (видимо эта часть выпилится в консольное приложение, но пока до конца не ясно).
            var exchangeRatesFactory = new ExchangeRatesFactory();
            IEnumerable<BankRatesDataModel> banks = exchangeRatesFactory.GetBankRatesDatas(requests);

            // Вывести полученные значения для одного банка для одной валюты. Будем пилить дальше.
            ShowResults(banks);
            Console.ReadKey();
        }

        /// <summary>
        /// Выводит полученные результаты в консоль.
        /// </summary>
        /// <param name="banks"></param>
        static void ShowResults (IEnumerable<BankRatesDataModel> banks)
        {
            foreach (var bank in banks)
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
        }
    }
}
