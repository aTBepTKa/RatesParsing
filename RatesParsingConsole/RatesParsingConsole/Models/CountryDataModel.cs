using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.Models
{
    // Подумать о том, чтобы выпилить этот класс к хуям собачьим.
    /// <summary>
    /// Наименование страны и государственной денежной еденицы.
    /// </summary>
    class CountryDataModel
    {
        /// <summary>
        /// Обозначение наименования страны (BRL).
        /// </summary>
        public string ShortName { get; set; }  

        /// <summary>
        /// Полное наименование страны (Республика Беларусь).
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Национальная валюта страны.
        /// </summary>
        public CurrencyDataModel NationalCurrency { get; set; }

        /// <summary>
        /// Наименование банка.
        /// </summary>
        public string BankName { get; set; }
    }
}
