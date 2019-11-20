using System;
using System.IO;
using HtmlAgilityPack;


namespace RatesParsingConsole
{
    /// <summary>
    /// Работа с входными данными.
    /// </summary>
    class InputData
    {
        /// <summary>
        /// Текст ошибки. 
        /// Чето какуюто хуйню похоже придумал. В класс Program пихать не хочется проверку исключений, хочется чтобы здесь все обрабатывалось.
        /// </summary>
        public string ErrorText { get; private set; }

        /// <summary>
        /// Получить html документ из файла.
        /// </summary>
        /// <param name="FilePath">Путь к файлу.</param>
        /// <returns></returns>
        public HtmlDocument GetHtmlDocumentFromFile(string FilePath)
        {
            var document = new HtmlDocument();
            try
            {
                document.Load(FilePath);
            }
            // Если файл не найден, возвращаем null.
            catch (FileNotFoundException)
            {
                ErrorText = $"Файл \"...\\{FilePath}\" не найден.";
                return null;
            }
            
            return document;
        }

        /// <summary>
        /// Получить html документ из сети.
        /// </summary>
        /// <param name="URL">Адрес получаемой страницы.</param>
        /// <returns></returns>
        public HtmlDocument GetHtmlDocumentFromWeb(string URL)
        {
            var web = new HtmlWeb();
            var document = web.Load(URL);
            return document;
        }
    }
}
