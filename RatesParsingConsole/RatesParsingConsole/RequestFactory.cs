using System;
using System.Collections.Generic;
using System.Text;
using RatesParsingConsole.Models;

namespace RatesParsingConsole
{
    /// <summary>
    /// Представляет средства для формирования запроса к банкам.
    /// </summary>
    class RequestFactory
    {
        /// <summary>
        /// Получить список запросов к банкам. Здесь будем парсить JSON.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BankDataRequestDto> GetBankDataRequests()
        {
            // Сформировать запрос к одному банку.
            var requestBankDataDto = new BankDataRequestDto();

            // Установить адрес запрашиваемой страницы и название банка.
            requestBankDataDto.RatesUrlPage = "https://www.nbg.gov.ge/index.php?m=582&lng=eng";

            // Назначить разделитель для числа.
            requestBankDataDto.NumberDecimalSeparator = ".";
            requestBankDataDto.NumberGroupSeparator = ",";

            // Получить сценарий парсинга.
            requestBankDataDto.XPathes = GetParsingScript();

            // Сформировать коллекцию запросов. Работаем пока с одним банком.
            var requestBankDataList = new List<BankDataRequestDto>
            {
                requestBankDataDto
            };

            return requestBankDataList;
        }

        /// <summary>
        /// Получить сценарий парсинга.
        /// </summary>
        /// <returns>XPath пути для данных по валюте.</returns>
        private CurrencyXPathesDto GetParsingScript()
        {
            var pathes = new CurrencyXPathesDto()
            {
                ShortName = @"//*[@id='currency_id']/table/tr[1]/td[1]",
                FullName = @"//*[@id='currency_id']/table/tr[1]/td[2]",
                Unit = "доработать",
                ExchangeRate = @"//*[@id='currency_id']/table/tr[1]/td[3]"
            };
            return pathes;
        }
    }
}
