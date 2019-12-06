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
            GetRequestType requestType = GetRequestType.FromCode;

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

            // Вывести полученные значения.
            ShowExchangeRates(banks);
            WriteToFileAsync(banks);

            // Тест рефлексии.
            var a = GetMethodsFromString("GetNumberFromText");
            var b = a("100 500 ABS 777");
            Console.WriteLine(b);

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
            // Сформировать число из найденных в строке цифр.
            // Для обработки текста используется делегат, в который передается готовый метод из класса ScriptCommands. 

            // Схема работы обработки текста:
            // - класс BankRequestDto содержит делегаты для обработки каждого полученного поля (TextCode, Unit, ExchangeRate),
            // - класс ScriptCommands содержит готовые методы для обработки текста.
            // - Таким образом при формировании запроса получаем нужные метода из класса ScriptCommands и передаем их в делегат класса BankRequestDto.
            bank1.GetUnitSubString = scriptCommands.GetNumberFromText();
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
            bank2.GetUnitSubString = scriptCommands.GetNumberFromText();
            // Сформировать текстовый код валюты получив последние три символа строки.
            var textCodeLength = 3;
            bank2.GetTextCodeSubString = scriptCommands.GetTextCodeFromEnd(textCodeLength);
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

        /// <summary>
        /// Получить методы из текстового списка.
        /// </summary>
        /// <param name="methodsString">Список методов.</param>
        /// <returns></returns>
        private static WordProcessingHandler GetMethodsFromString(string methodName)
        {
            // Получить необходимый тип.
            Type t = typeof(ScriptCommands);
            // Получить необходимый метод.
            object obj = Activator.CreateInstance(t);
            MethodInfo method = t.GetMethod(methodName);


            WordProcessingHandler NewMethod = method.Invoke(obj, new object[] {  }) as WordProcessingHandler;
            return NewMethod;
        }

        private enum GetRequestType { FromFile, FromCode }
    }
}
