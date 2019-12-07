using System;
using System.Collections.Generic;
using System.Text;

namespace RatesParsingConsole.DTO
{
    /// <summary>
    /// Данные результата обработки данных.
    /// </summary>
    public class ProcessingResultDto
    {
        /// <summary>
        /// Тип результата обработки.
        /// </summary>
        public enum ResultType { None, Success, ProcessedWithErrors, Error }

        /// <summary>
        /// Результат обработки запроса.
        /// </summary>
        public ResultType RequestResultStatus { get; set; }

        /// <summary>
        /// Описание результата обработки запроса.
        /// </summary>
        public string RequestResultMessage { get; set; } = "";
    }
}
