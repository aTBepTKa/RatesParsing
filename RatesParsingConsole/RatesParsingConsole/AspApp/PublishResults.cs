using System;
using System.IO;
using System.Collections.Generic;
using RatesParsingConsole.AspApp.Models;
using static RatesParsingConsole.AspApp.Models.ProcessingResult;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Mapster;
using RatesParsingConsole.DTO.Json;
using System.Linq;

namespace RatesParsingConsole.AspApp
{
    /// <summary>
    /// Опубликовать результаты работы программы.
    /// </summary>
    class PublishResults
    {
        /// <summary>
        /// Выводит полученные результаты в консоль.
        /// </summary>
        /// <param name="ExchangeRates"></param>
        public void ShowExchangeRates(IEnumerable<BankRates> banks)
        {
            foreach (var bank in banks)
            {

                Console.WriteLine($"{Environment.NewLine}Курсы валют банка \"{bank.Name}\", " +
                    $"национальная валюта {bank.Currency}:");
                Console.WriteLine();
                foreach (var Rate in bank.ExchangeRates)
                {
                    if (Rate.RequestResultStatus == ResultType.Success)
                    {
                        Console.WriteLine($"Код валюты: {Rate.TextCode}");
                        Console.WriteLine($"Единица: {Rate.Unit}");
                        Console.WriteLine($"Обменный курс: {Rate.ExchangeRateValue}");
                        Console.WriteLine();
                    }
                    else
                        Console.WriteLine($"Ошибка при получении данных валюты: {Rate.RequestResultMessage}.");
                }
                Console.WriteLine();
            }

        }

        /// <summary>
        /// Записать полученные данные в файл асинхронно.
        /// </summary>
        /// <param name="bankData"></param>
        public async void WriteToFileAsync(IEnumerable<BankRates> banks)
        {
            var fileName = "ExchangeRates.txt";
            try
            {
                FileStream stream = new FileStream(fileName, FileMode.Create);
                using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
                {
                    foreach (var bank in banks)
                    {
                        await sw.WriteLineAsync($"Курсы валют банка \"{bank.Name}\", " +
                            $"национальная валюта {bank.Currency}:");
                        await sw.WriteLineAsync();
                        foreach (var Rate in bank.ExchangeRates)
                        {
                            if (Rate.RequestResultStatus == ResultType.Success)
                            {
                                await sw.WriteLineAsync($"Код валюты: {Rate.TextCode}");
                                await sw.WriteLineAsync($"Единица: {Rate.Unit}");
                                await sw.WriteLineAsync($"Обменный курс: {Rate.ExchangeRateValue}");
                                await sw.WriteLineAsync();
                            }
                            else
                                await sw.WriteLineAsync($"Ошибка при получении данных валюты: {Rate.RequestResultMessage}");
                        }
                        await sw.WriteLineAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
            }
        }

        /// <summary>
        /// Сериализовать результаты парсинга в .json файл.
        /// </summary>
        /// <param name="banks"></param>
        /// <returns></returns>
        public async Task SerializeToJsonFile(IEnumerable<BankRates> banks)
        {
            var bankJsonList = new List<BankJson>(banks.Count());
            try
            {
                // Сформировать коллекцию банков с обменными курсами за несколько дней.
                foreach (var bank in banks)
                {
                    var bankJson = bank.Adapt<BankJson>();

                    // Заполнить данные обменных курсов за четыре дня фиктивными данными за один день.
                    DateTime date = new DateTime(2020, 2, 6);
                    int exchangeRateListsCount = 4;
                    var exchangeRateLists = new List<ExchangeRateListJson>(exchangeRateListsCount);
                    var exchangeRates = bank.ExchangeRates.Adapt<IEnumerable<ExchangeRateJson>>();
                    for (int i = 0; i < exchangeRateListsCount; i++)
                    {
                        ExchangeRateListJson exchangeRateListJson = new ExchangeRateListJson
                        {
                            DateTimeStamp = date.AddDays(-i),
                            ExchangeRates = exchangeRates
                        };
                        exchangeRateLists.Add(exchangeRateListJson);
                    }
                    bankJson.ExchangeRateLists = exchangeRateLists;
                    bankJsonList.Add(bankJson);
                }
                var fileName = "FakeExchangeRates.json";
                using FileStream fileStream = new FileStream(fileName, FileMode.CreateNew);
                var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
                await JsonSerializer.SerializeAsync(fileStream, bankJsonList, serializerOptions);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении сериализации {ex.Message}");
            }
            Console.WriteLine("Сериализация в .json прошла успешно.");
        }
    }
}
