using System;
using System.IO;
using System.Collections.Generic;
using RatesParsingConsole.Models;

namespace RatesParsingConsole
{
    internal class Program
    {
        /// <summary>
        /// Получает курсы обмена валют с различных банков и выводит в консоль.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Формируем список банков с запросами.
            var bankDataModelList = new List<BankDataModel>();
            bankDataModelList.AddRange(GetBankData());

            // Инструмент для получения результатов парсинга страниц.
            var ratesFactory = new ExchangeRatesFactory();

            // Получить курсы валют.
            foreach (var bank in bankDataModelList)
                bank.BankRates.ExchangeRates = ratesFactory.GetBankRatesDatas(bank.BankDataRequest);

            // Вывести полученные значения.
            ShowExchangeRates(bankDataModelList);
            WriteToFile(bankDataModelList);

            Console.ReadKey();
        }

        /// <summary>
        /// Сформировать список банков с данными запроса.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<BankDataModel> GetBankData()
        {
            // TODO: реализовать работу с данными JSON.

            // Список банков.
            var bankDataModels = new List<BankDataModel>();

            // Установить данные для банков.

            // Данные для Банка 1.
            // TODO: Разработать БД с информацией по банкам и соответствующими запросами.
            var bank1 = new BankDataModel();
            bank1.BankRates.BankName = "National Bank of Georgia";
            bank1.BankRates.BankCurrency = "GEL";
            bank1.BankDataRequest.RatesUrlPage = "https://www.nbg.gov.ge/index.php?m=582&lng=eng";
            bank1.BankDataRequest.NumberDecimalSeparator = ".";
            bank1.BankDataRequest.NumberGroupSeparator = ",";
            // XPath пути для валюты.
            // Данные для реализации цикла перебора строк с курсами валют.
            var StartRow1 = 1;
            var RowsNum1 = 43;
            bank1.BankDataRequest.StartRow = StartRow1;
            bank1.BankDataRequest.RowsNum = RowsNum1;
            bank1.BankDataRequest.VariablePartOfXpath = "$VARIABLE";
            // Шаблон пути.
            bank1.BankDataRequest.XPathes = new CurrencyXPathesDto()
            {
                TextCode = @"//*[@id='currency_id']/table/tr[$VARIABLE]/td[1]",
                Unit = @"//*[@id='currency_id']/table/tr[$VARIABLE]/td[2]",
                ExchangeRate = @"//*[@id='currency_id']/table/tr[$VARIABLE]/td[3]"
            };
            // Получить число (единицу измерения валюты) из строки с текстом.
            bank1.BankDataRequest.GetUnitSubString = delegate (string text)
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


            // Данные для Банка 2.
            var bank2 = new BankDataModel();
            bank2.BankRates.BankName = "National Bank of Poland";
            bank2.BankRates.BankCurrency = "PLN";
            bank2.BankDataRequest.RatesUrlPage = "https://www.nbp.pl/homen.aspx?f=/kursy/RatesA.html";
            bank2.BankDataRequest.NumberDecimalSeparator = ".";
            bank2.BankDataRequest.NumberGroupSeparator = ",";
            var StartRow2 = 2;
            var RowsNum2 = 36;
            bank2.BankDataRequest.StartRow = StartRow2;
            bank2.BankDataRequest.RowsNum = RowsNum2;
            bank2.BankDataRequest.VariablePartOfXpath = "$VARIABLE";
            bank2.BankDataRequest.XPathes = new CurrencyXPathesDto()
            {
                TextCode = @"//*[@id=""article""]/table/tr/td/center/table[1]/tr[$VARIABLE]/td[2]",
                Unit = @"//*[@id=""article""]/table/tr/td/center/table[1]/tr[$VARIABLE]/td[2]",
                ExchangeRate = @"//*[@id=""article""]/table/tr/td/center/table[1]/tr[$VARIABLE]/td[3]"
            };
            // Сформировать число из найденных в строке цифр.
            bank2.BankDataRequest.GetUnitSubString = delegate (string text)
            {
                string digitText = "";

                foreach (char ch in text)
                {
                    if (char.IsDigit(ch))
                        digitText += ch;
                }

                return digitText;
            };
            // Сформировать текстовый код валюты получив последние три символа строки.
            bank2.BankDataRequest.GetTextCodeSubString = delegate (string text)
            {
                var CodeLength = 3;
                var NewString = text.Substring(text.Length - CodeLength);
                return NewString;
            };
            bankDataModels.Add(bank2);

            // Банк 3.
            var bank3 = new BankDataModel();
            bank3.BankRates.BankName = "The Central Bank of the Russian Federation";
            bank3.BankRates.BankCurrency = "RUB";
            bank3.BankDataRequest.RatesUrlPage = "https://www.cbr.ru/eng/currency_base/daily/";
            bank3.BankDataRequest.NumberDecimalSeparator = ".";
            bank3.BankDataRequest.NumberGroupSeparator = ",";
            var StartRow3 = 2;
            var RowsNum3 = 35;
            bank3.BankDataRequest.StartRow = StartRow3;
            bank3.BankDataRequest.RowsNum = RowsNum3;
            bank3.BankDataRequest.VariablePartOfXpath = "$VARIABLE";
            bank3.BankDataRequest.XPathes = new CurrencyXPathesDto()
            {
                TextCode = @"//*[@id=""content""]/table/tbody/tr[$VARIABLE]/td[2]",
                Unit = @"//*[@id=""content""]/table/tbody/tr[$VARIABLE]/td[3]",
                ExchangeRate = @"//*[@id=""content""]/table/tbody/tr[$VARIABLE]/td[5]"
            };
            bankDataModels.Add(bank3);

            return bankDataModels;
        }

        /// <summary>
        /// Выводит полученные результаты в консоль.
        /// </summary>
        /// <param name="ExchangeRates"></param>
        private static void ShowExchangeRates(IEnumerable<BankDataModel> bankDataModelList)
        {
            foreach (var bank in bankDataModelList)
            {
                Console.WriteLine($"{Environment.NewLine}Курсы валют банка \"{bank.BankRates.BankName}\", " +
                    $"национальная валюта {bank.BankRates.BankCurrency}:");
                Console.WriteLine();
                foreach (var Rate in bank.BankRates.ExchangeRates)
                {
                    if (Rate.IsSuccessfullyParsed)
                    {
                        Console.WriteLine($"Код валюты: {Rate.TextCode}");
                        Console.WriteLine($"Единица: {Rate.Unit}");
                        Console.WriteLine($"Обменный курс: {Rate.ExchangeRate}");
                        Console.WriteLine();
                    }
                    else
                        Console.WriteLine($"Ошибка при получении данных валюты: {Rate.ErrorName}.");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Записать полученные данные в файл.
        /// </summary>
        /// <param name="bankData"></param>
        private static async void WriteToFile(IEnumerable<BankDataModel> bankDataModelList)
        {
            // Задать объект потока для записи и задать имя файла.
            FileStream stream = null;
            var fileName = "ExchangeRates.txt";

            try
            {
                // Создать поток.
                stream = new FileStream(fileName, FileMode.OpenOrCreate);
                // Записать данные в файл.
                using (StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.UTF8))
                {
                    foreach (var bank in bankDataModelList)
                    {
                        await sw.WriteLineAsync($"Курсы валют банка \"{bank.BankRates.BankName}\", " +
                            $"национальная валюта {bank.BankRates.BankCurrency}:");
                        await sw.WriteLineAsync();
                        foreach (var Rate in bank.BankRates.ExchangeRates)
                        {
                            if (Rate.IsSuccessfullyParsed)
                            {
                                await sw.WriteLineAsync($"Код валюты: {Rate.TextCode}");
                                await sw.WriteLineAsync($"Единица: {Rate.Unit}");
                                await sw.WriteLineAsync($"Обменный курс: {Rate.ExchangeRate}");
                                await sw.WriteLineAsync();
                            }
                            else
                                await sw.WriteLineAsync($"Ошибка при получении данных валюты: {Rate.ErrorName}.");
                        }
                        await sw.WriteLineAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
            }
            finally
            {
                // Освободить ресурсы потока.
                if (stream != null)
                    stream.Dispose();
            }
        }
    }
}
