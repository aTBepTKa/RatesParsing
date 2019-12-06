using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using RatesParsingConsole.Models;
using System.Reflection;

namespace RatesParsingConsole
{
    /// <summary>
    /// Обработчик для преобразования строки.
    /// </summary>
    /// <param name="text">Исходный текст.</param>
    /// <returns></returns>
    public delegate string WordProcessingHandler(string text);

    class Program
    {
        /// <summary>
        /// Получает курсы обмена валют с различных банков.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Временно. Переключатель получения запроса: из файла или из кода.
            GetRequestType requestType = GetRequestType.FromFile;

            // Формируем список запросов к бакнкам.
            IEnumerable<BankRequestDto> requests;

            if (requestType == GetRequestType.FromCode)
                requests = GetBankRequestsFromCode();
            else if (requestType == GetRequestType.FromFile)
            {
                var requestsFileName = @"D:\Projects\RatesParsing\RatesParsingConsole\RatesParsingConsole\Scripts\Requests.json";
                requests = GetBankRequestsFromFile(requestsFileName);
            }
            else
                requests = null;

            // Получить данные курсов по банкам асинхронно.
            IEnumerable<BankRatesModel> banks = GetBankRatesAsync(requests).Result;

            // Получить данные курсов по банкам синхронно.
            //IEnumerable<BankRatesModel> banks = GetBankRates(requests);

            // Вывести полученные значения.
            ShowExchangeRates(banks);
            WriteToFileAsync(banks);

            Console.ReadKey();
        }

        // Попытка запустить парсинг сайтов асинхронно. Не понятно работает ли.
        /// <summary>
        /// Получить данные банков с обменными курсами асинхронно.
        /// </summary>
        /// <param name="requests">Список запросов.</param>
        /// <returns></returns>
        private static async Task<IEnumerable<BankRatesModel>> GetBankRatesAsync(IEnumerable<BankRequestDto> requests)
        {
            // Инструмент для обработки запроса и получения данных страниц банков.
            var factory = new ExchangeRatesFactory();

            // Список задач.
            var tasks = new List<Task<BankRatesModel>>();

            // Получить данные обменных курсов по каждому банку асинхронно. 
            // (запустить парсинг каждого сайта параллельно)
            foreach (var req in requests)
                tasks.Add(factory.GetBankRatesAsync(req));

            // Подождать завершения всех задач и получить спиок банков с курсами.
            IEnumerable<BankRatesModel> banks = await Task.WhenAll(tasks);

            return banks;
        }

        /// <summary>
        /// Получить данные банков с обменными курсами.
        /// </summary>
        /// <param name="requests">Список запросов.</param>
        /// <returns></returns>
        private static IEnumerable<BankRatesModel> GetBankRates(IEnumerable<BankRequestDto> requsts)
        {
            // Список банков.
            List<BankRatesModel> banks = new List<BankRatesModel>();
            // Инструмент для обработки запроса и получения данных страниц банков.
            var factory = new ExchangeRatesFactory();
            // Получить данные банка по каждому запросу.
            foreach (var req in requsts)
                banks.Add(factory.GetBankRates(req));
            return banks;
        }

        /// <summary>
        /// Получить список запросов к банкам из кода.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<BankRequestDto> GetBankRequestsFromCode()
        {
            // TODO: реализовать работу с данными JSON.

            // Список банков.
            var bankDataModels = new List<BankRequestDto>();
            // Содержит методы для обработки текста.
            ScriptCommands scriptCommands = new ScriptCommands();

            // Установить данные для банков.
            // Переменная часть пути XPath для всех банков.
            var VariablePartOfXpath = "$VARIABLE";


            // Данные для Банка 1.
            // Данные для реализации цикла перебора строк с курсами валют.
            var StartRow1 = 1;
            // Последняя строка задана на 2 больше, чтобы проверить отработку ошибок (должно быть 43).
            var EndRow1 = 45;
            var bank1 = new BankRequestDto
            {
                BankName = "National Bank of Georgia",
                BankCurrency = "GEL",
                RatesUrlPage = "https://www.nbg.gov.ge/index.php?m=582&lng=eng",
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = ",",
                StartRow = StartRow1,
                EndRow = EndRow1,
                VariablePartOfXpath = VariablePartOfXpath
            };
            // Шаблон XPath пути для валюты.            
            bank1.XPathes = new CurrencyXPathesDto()
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
            var bank2 = new BankRequestDto
            {
                BankName = "National Bank of Poland",
                BankCurrency = "PLN",
                RatesUrlPage = "https://www.nbp.pl/homen.aspx?f=/kursy/RatesA.html",
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = ",",
                StartRow = StartRow2,
                EndRow = EndRow2,
                VariablePartOfXpath = VariablePartOfXpath
            };
            bank2.XPathes = new CurrencyXPathesDto()
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
            var bank3 = new BankRequestDto
            {
                BankName = "The Central Bank of the Russian Federation",
                BankCurrency = "RUB",
                RatesUrlPage = "https://www.cbr.ru/eng/currency_base/daily/",
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = ",",
                StartRow = StartRow3,
                EndRow = EndRow3,
                VariablePartOfXpath = VariablePartOfXpath
            };
            bank3.XPathes = new CurrencyXPathesDto()
            {
                TextCode = @"//*[@id='content']/table/tbody/tr[$VARIABLE]/td[2]",
                Unit = @"//*[@id='content']/table/tbody/tr[$VARIABLE]/td[3]",
                ExchangeRate = @"//*[@id='content']/table/tbody/tr[$VARIABLE]/td[5]"
            };
            bankDataModels.Add(bank3);

            return bankDataModels;
        }

        /// <summary>
        /// Получить список запросов к банкам из .json файла
        /// </summary>
        /// <param name="fileName">Путь к файлу .json.</param>
        /// <returns></returns>
        private static IEnumerable<BankRequestDto> GetBankRequestsFromFile(string fileName)
        {
            // TODO: обработать ошибки чтения .json файла.
            IEnumerable<BankRequestDto> requests;

            string JsonText;
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.UTF8))
                    JsonText = sr.ReadToEnd();
                requests = JsonSerializer.Deserialize<IEnumerable<BankRequestDto>>(JsonText);
                return requests;
            }
            else
            {
                return Array.Empty<BankRequestDto>();
            }
        }

        /// <summary>
        /// Выводит полученные результаты в консоль.
        /// </summary>
        /// <param name="ExchangeRates"></param>
        private static void ShowExchangeRates(IEnumerable<BankRatesModel> banks)
        {
            foreach (var bank in banks)
            {
                Console.WriteLine($"{Environment.NewLine}Курсы валют банка \"{bank.BankName}\", " +
                    $"национальная валюта {bank.BankCurrency}:");
                Console.WriteLine();
                foreach (var Rate in bank.ExchangeRates)
                {
                    if (Rate.RequestResultStatus == ProcessingResultModel.ProcessingResult.Success)
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
        private static async void WriteToFileAsync(IEnumerable<BankRatesModel> banks)
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
                            if (Rate.RequestResultStatus == ProcessingResultModel.ProcessingResult.Success)
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

        private enum GetRequestType { FromFile, FromCode }
    }
}
