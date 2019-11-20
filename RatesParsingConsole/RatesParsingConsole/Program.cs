using System;
using HtmlAgilityPack;

namespace RatesParsingConsole
{
    class Program
    {
        // Загрузить html файл с жесткого диска и вывести в консоль заголовок страницы. То же самое проделать со страницей из интернета.
        static void Main(string[] args)
        {
            // Установить параметры для парсинга (имя файла и адрес страницы).
            var parseSettings = new ParseSettings
            {
                HttpFileName = @"Other\Sites\Georgia\National Bank Of Georgia.html",
                HttpUrl = "https://www.nbg.gov.ge/index.php?m=582&lng=eng"
            };

            // Получить инструкции JSON. 
            // Пока не пилил, просто пропишем XPath.
            string xPath = "/html/head/title";

            // Инструмент для получения html страниц.
            var gettingHtml = new GettingHtml(parseSettings);

            // Получить html документ из файла.
            Console.WriteLine("Получить документ из файла:");
            HtmlDocument HtmlDocFile = gettingHtml.GetHtmlDocumentFromFile();
            // При успешном открытии вывести заголовок документа.            
            if (HtmlDocFile != null)
            {
                Console.WriteLine("Заголовок страницы из файла:");
                Console.WriteLine(GetHtmlHead(HtmlDocFile, xPath));
            }
            Console.WriteLine();
            
            
            // Получить html документ из интернета.      
            Console.WriteLine("Нажмите любую клавишу для загрузки страницы из интернета.");
            Console.ReadKey();
            Console.WriteLine("\n\n");
            Console.WriteLine("Получить документ из интернета:");
            HtmlDocument HtmlDocWeb = gettingHtml.GetHtmlDocumentFromWeb();
            // При успешном открытии вывести заголовок документа.            
            if (HtmlDocWeb != null)
            {
                Console.WriteLine("Заголовок страницы из интернета:");
                Console.WriteLine(GetHtmlHead(HtmlDocWeb, xPath));
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Получить заголовок страницы.
        /// </summary>
        /// <param name="htmlDocument">Объект html документа.</param>
        /// <param name="xPath">Текст внутри тега по адресу XPath.</param>
        /// <returns></returns>
        private static string GetHtmlHead(HtmlDocument htmlDocument, string xPath)
        {
            //Console.WriteLine();
            string HtmlHead = htmlDocument.DocumentNode.SelectSingleNode(xPath).InnerText;
            Console.WriteLine();
            return HtmlHead;
        }
    }
}
