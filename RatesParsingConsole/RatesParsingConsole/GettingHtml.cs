using System;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace RatesParsingConsole
{
    /// <summary>
    /// Работа с входными данными.
    /// </summary>
    class GettingHtml
    {
        /// <summary>
        /// Получить html документ из файла.
        /// </summary>
        /// <param name="FileName">Путь к файлу.</param>
        /// <returns></returns>
        public HtmlDocument GetHtmlDocumentFromFile(string FileName)
        {
            var document = new HtmlDocument();
            try
            {
                document.Load(FileName);
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Ошибка при открытии файла \"...\\{FileName}\":{Environment.NewLine}" +
                    $"{e.GetType().Name}");
                return null;
            }
            Console.WriteLine($"Файл \"...\\{FileName}\" успешно открыт.{Environment.NewLine}");
            return document;
        }

        /// <summary>
        /// Получить html документ из интернета асинхронно.
        /// </summary>
        /// <param name="URL">Адрес получаемой страницы.</param>
        /// <returns></returns>
        public async Task<HtmlDocument> GetHtmlFromWebAsync(string URL)
        {
            return await Task.Run(() => GetHtmlFromWeb(URL));
        }

        /// <summary>
        /// Получить html документ из интернета.
        /// </summary>
        /// <param name="URL">Адрес получаемой страницы.</param>
        /// <returns></returns>
        public HtmlDocument GetHtmlFromWeb(string URL)
        {
            var web = new HtmlWeb();
            var document = new HtmlDocument();
            try
            {
                document = web.Load(URL);
            }
            catch (UriFormatException)
            {
                Console.WriteLine($"Не верный формат URL ({URL}).{Environment.NewLine}");
                return null;
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine($"Запрашиваемая страница не найдена ({URL}).{Environment.NewLine}");
                return null;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine($"Запрашиваемая страница не задана.{Environment.NewLine}");
                return null;
            }

            Console.WriteLine($"Страница {URL} успешно загружена.");
            return document;
        }
    }
}
