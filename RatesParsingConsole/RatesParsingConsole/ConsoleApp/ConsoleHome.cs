using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RatesParsingConsole.DTO;

namespace RatesParsingConsole.ConsoleApp
{
    /// <summary>
    /// Обработчик для преобразования строки.
    /// </summary>
    /// <param name="text">Исходный текст.</param>
    /// <returns></returns>
    public delegate string WordProcessingHandler(string text);

    /// <summary>
    /// Точка входа в консольное приложение.
    /// </summary>
    public class ConsoleHome
    {
        /// <summary>
        /// Получить данные банков с обменными курсами асинхронно.
        /// </summary>
        /// <param name="requests">Список запросов.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BankRatesDto>> GetBankRatesAsync(IEnumerable<BankRequestDto> requests)
        {
            // Инструмент для обработки запроса и получения данных страниц банков.
            var factory = new ExchangeRatesFactory();

            // Список задач.
            var tasks = new List<Task<BankRatesDto>>(requests.Count());

            // Получить данные обменных курсов по каждому банку асинхронно. 
            // (запустить парсинг каждого сайта параллельно)
            foreach (var req in requests)
                tasks.Add(factory.GetBankRatesAsync(req));

            // Подождать завершения всех задач и получить спиок банков с курсами.
            IEnumerable<BankRatesDto> banks = await Task.WhenAll(tasks);

            return banks;
        }

        /// <summary>
        /// Получить данные банков с обменными курсами.
        /// </summary>
        /// <param name="requests">Список запросов.</param>
        /// <returns></returns>
        public IEnumerable<BankRatesDto> GetBankRates(IEnumerable<BankRequestDto> requsts)
        {
            // Список банков.
            List<BankRatesDto> banks = new List<BankRatesDto>();
            // Инструмент для обработки запроса и получения данных страниц банков.
            var factory = new ExchangeRatesFactory();
            // Получить данные банка по каждому запросу.
            foreach (var req in requsts)
                banks.Add(factory.GetBankRates(req));
            return banks;
        }
    }
}
