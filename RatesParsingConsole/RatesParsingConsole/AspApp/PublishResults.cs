using System;
using System.IO;
using System.Collections.Generic;
using RatesParsingConsole.AspApp.Models;
using static RatesParsingConsole.AspApp.Models.ProcessingResult;

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
                
                Console.WriteLine($"{Environment.NewLine}Курсы валют банка \"{bank.BankName}\", " +
                    $"национальная валюта {bank.BankCurrency}:");
                Console.WriteLine();
                foreach (var Rate in bank.ExchangeRates)
                {
                    if (Rate.RequestResultStatus == ResultType.Success)
                    {
                        Console.WriteLine($"Код валюты: {Rate.TextCode}");
                        Console.WriteLine($"Единица: {Rate.Unit}");
                        Console.WriteLine($"Обменный курс: {Rate.ExchangeRate}");
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
            FileStream stream = null;
            var fileName = "ExchangeRates.txt";
            try
            {
                stream = new FileStream(fileName, FileMode.Create);
                using (StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.UTF8))
                {
                    foreach (var bank in banks)
                    {
                        await sw.WriteLineAsync($"Курсы валют банка \"{bank.BankName}\", " +
                            $"национальная валюта {bank.BankCurrency}:");
                        await sw.WriteLineAsync();
                        foreach (var Rate in bank.ExchangeRates)
                        {
                            if (Rate.RequestResultStatus == ResultType.Success)
                            {
                                await sw.WriteLineAsync($"Код валюты: {Rate.TextCode}");
                                await sw.WriteLineAsync($"Единица: {Rate.Unit}");
                                await sw.WriteLineAsync($"Обменный курс: {Rate.ExchangeRate}");
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
    }
}
