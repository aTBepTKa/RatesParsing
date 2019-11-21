using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using RatesParsingConsole.RateDataBase;

namespace RatesParsingConsole
{
    class Program
    {
        /// <summary>
        /// Получает курсы обмена валют с различных банков и сохраняет в базу (то что в будущем будет делать MVC приложение).
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Коллекция курсов по каждому банку (типа БД).
            List<BankRatesData> Banks = new List<BankRatesData>();

            // Список запросов к банкам.
            List<RequestBankData> requests = GetRequestBankDatas();

            // Перебрать список запросов и получить данные по валютам.
            foreach (var req in requests)
            {
                Banks.Add(GetBankRatesData(req));
            }
        }

        /// <summary>
        /// Получить список запросов к банкам.
        /// </summary>
        /// <returns></returns>
        public static List<RequestBankData> GetRequestBankDatas()
        {
            return null;
        }

        /// <summary>
        /// Получить данные курсов конкретного банка.
        /// </summary>
        /// <param name="requestBankData">Данные для получения курса</param>
        /// <returns></returns>
        public static BankRatesData GetBankRatesData(RequestBankData requestBankData)
        {
            return null;
        }

    }
}
