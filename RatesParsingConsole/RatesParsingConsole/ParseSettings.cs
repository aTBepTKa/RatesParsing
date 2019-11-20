using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole
{
    /// <summary>
    /// Содержит настройки для выполнения парсинга.
    /// </summary>
    class ParseSettings
    {
        /// <summary>
        /// Имя локального .http файла.
        /// </summary>
        public string HttpFileName { get; set; }

        /// <summary>
        /// Ссылка на .http страницу в интернете.
        /// </summary>
        public string HttpUrl { get; set; }
    }
}
