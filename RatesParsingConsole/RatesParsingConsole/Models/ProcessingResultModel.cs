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
        public ProcessingResult RequestResultStatus
        {
            get
            {
                return requestResultStatus;
            }
            set
            {
                requestResultStatus = value;
                switch (requestResultStatus)
                {
                    case ProcessingResult.Success:
                        RequestResultMessage = "Данные успешно получены.";
                        return;
                    case ProcessingResult.ProcessedWithErrors:
                        RequestResultMessage = "Завершено с ошибками.";
                        return;
                    case ProcessingResult.Error:
                        RequestResultMessage = "Ошибка при получении данных.";
                        return;
                    case ProcessingResult.None:
                        RequestResultMessage = "Данные о результате обработки запроса отсутствуют.";
                        return;
                }
            }
        }
        private ProcessingResult requestResultStatus;

        /// <summary>
        /// Описание результата обработки запроса.
        /// </summary>
        public string RequestResultMessage { get; set; } = "";
    }
}
