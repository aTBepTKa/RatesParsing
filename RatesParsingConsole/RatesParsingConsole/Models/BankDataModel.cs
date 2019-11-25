using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.Models
{
    /// <summary>
    /// Содержит банк и соответствующий ему запрос для получения данных по курсам.
    /// </summary>
    class BankDataModel
    {
        /// <summary>
        /// Курсы валют.
        /// </summary>
        public BankRatesDataModel BankRates { get; set; }

        /// <summary>
        /// Запрос.
        /// </summary>
        public BankDataRequestDto BankDataRequest { get; set; }

        public BankDataModel()
        {
            BankRates = new BankRatesDataModel();
            BankDataRequest = new BankDataRequestDto();
        }
    }
}
