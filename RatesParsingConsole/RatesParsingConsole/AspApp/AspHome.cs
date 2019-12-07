using System;
using System.Collections.Generic;
using RatesParsingConsole.DTO;
using RatesParsingConsole.ConsoleApp;
using RatesParsingConsole.AspApp.Models;
using Mapster;

namespace RatesParsingConsole.AspApp
{
    /// <summary>
    /// Точка входа в ASP приложение (выпилится в отдельный солюшн).
    /// </summary>
    class AspHome
    {
        public void StartApp()
        {
            // Временно. Переключатель получения запроса: из файла или из кода.
            GetRequestType requestType = GetRequestType.FromCode;

            // Получить список запросов.
            RequestFactory requestFactory = new RequestFactory();
            IEnumerable<BankRequest> requests;
            switch (requestType)
            {
                case GetRequestType.FromFile:
                    var requestsFileName = @"D:\Projects\RatesParsing\RatesParsingConsole\RatesParsingConsole\AspApp\Models\Requests.json";
                    requests = requestFactory.GetBankRequestsFromFile(requestsFileName);
                    break;
                case GetRequestType.FromCode:
                    requests = requestFactory.GetBankRequestsFromCode();
                    break;
                default:
                    requests = Array.Empty<BankRequest>();
                    break;
            }

            // Передать данные запроса в консольное приложение и получить результаты.
            // В процессе конвертировать данные из domain в dto и при получении произвести обратную конвертацию: из dto в domain.

            ConsoleHome consoleHome = new ConsoleHome();
            var requestsDto = requests.Adapt<IEnumerable<BankRequestDto>>();
            IEnumerable<BankRatesDto> banksDto = consoleHome.GetBankRatesAsync(requestsDto).Result;
            var banks = banksDto.Adapt<IEnumerable<BankRates>>();

            // Получить данные курсов по банкам синхронно.
            //IEnumerable<BankRatesDto> banks = consoleHome.GetBankRates(requests);

            // Вывести полученные значения.
            var publishResults = new PublishResults();
            publishResults.ShowExchangeRates(banks);
            publishResults.WriteToFileAsync(banks);

            Console.ReadKey();
        }

        /// <summary>
        /// Тип получения запроса.
        /// </summary>
        private enum GetRequestType { FromFile, FromCode }
    }
}
