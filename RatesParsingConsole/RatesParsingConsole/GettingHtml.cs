using System;
using HtmlAgilityPack;


namespace RatesParsingConsole
{
    /// <summary>
    /// Работа с входными данными.
    /// </summary>
    class GettingHtml
    {
        /// <summary>
        /// Настройки для получения html документа.
        /// </summary>
        public ParseSettings Settings { get; set; }

        public GettingHtml() { }

        /// <summary>
        /// Установить настройки получения .html документа.
        /// </summary>
        /// <param name="parse">Содержит настройки для парсинга.</param>
        public GettingHtml(ParseSettings parse)
        {
            Settings = parse;
        }

        /// <summary>
        /// Получить html документ из файла, указанного в свойстве HttpFileName в настройках парсинга.
        /// </summary>
        /// <returns></returns>
        public HtmlDocument GetHtmlDocumentFromFile()
        {
            return getHtmlDocumentFromFile(Settings.HttpFileName);
        }

        /// <summary>
        /// Получить html документ из файла.
        /// </summary>
        /// <param name="FilePath">Путь к файлу.</param>
        /// <returns></returns>
        public HtmlDocument GetHtmlDocumentFromFile(string FileName)
        {
            return getHtmlDocumentFromFile(FileName);
        }

        /// <summary>
        /// Получить html документ из интернета.
        /// </summary>
        /// <returns></returns>
        public HtmlDocument GetHtmlDocumentFromWeb()
        {
            return getHtmlDocumentFromWeb(Settings.HttpUrl);
        }

        /// <summary>
        /// Получить html документ из интернета.
        /// </summary>
        /// <param name="URL">Адрес получаемой страницы.</param>
        /// <returns></returns>
        public HtmlDocument GetHtmlDocumentFromWeb(string URL)
        {
            return getHtmlDocumentFromWeb(URL);
        }

        /// <summary>
        /// Метод для реализации перегрузки получения документа из файла.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private HtmlDocument getHtmlDocumentFromFile(string FileName)
        {
            var document = new HtmlDocument();
            try
            {
                document.Load(Settings.HttpFileName);
            }
            catch
            {
                Console.WriteLine($"Файл \"...\\{Settings.HttpFileName}\" не найден.\n");
                return null;
            }
            Console.WriteLine($"Файл \"...\\{Settings.HttpFileName}\" успешно открыт.\n");
            return document;
        }
        /// <summary>
        /// Метод для реализации перегрузки получения документа из интернета.
        /// </summary>
        /// <param name="URL">Адрес страницы.</param>
        /// <returns></returns>
        private HtmlDocument getHtmlDocumentFromWeb(string URL)
        {
            var web = new HtmlWeb();
            var document = new HtmlDocument();
            try
            {
                document = web.Load(Settings.HttpUrl);
            }
            catch (UriFormatException)
            {
                Console.WriteLine($"Не верный формат URL ({URL}).\n");
                return null;
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine($"Запрашиваемая страница не найдена ({URL}).\n");
                return null;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine($"Запрашиваемая страница не задана.\n");
                return null;
            }
            
            Console.WriteLine($"Страница {URL} успешно загружена.\n");
            return document;
        }
    }
}
