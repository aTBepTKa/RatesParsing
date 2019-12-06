using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.Models
{
    /// <summary>
    /// Данные результата обработки данных.
    /// </summary>
    class ProcessingResultModel
    {
        /// <summary>
        /// Тип результата обработки.
        /// </summary>
        public enum ProcessingResult { None, Success, ProcessedWithErrors, Error }

        /// <summary>
        /// Результат обработки запроса.
        /// </summary>
        public ProcessingResult RequestResultStatus { get; set; }

        /// <summary>
        /// Описание результата обработки запроса.
        /// </summary>
        public string RequestResultMessage { get; set; } = "";
    }
}
