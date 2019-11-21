using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.RateDataBase
{
    /// <summary>
    /// Наименование страны и государственной денежной еденицы.
    /// </summary>
    class CountryData
    {
        /// <summary>
        /// Обозначение наименования страны (BLR).
        /// </summary>
        public string ShortName { get; set; }  

        /// <summary>
        /// Полное наименование страны (Республика Беларусь).
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Национальная валюта страны.
        /// </summary>
        public CurrencyData NationalCurrency { get; set; }

        /// <summary>
        /// Наименование банка.
        /// </summary>
        public string BankName { get; set; }
    }
}
