using System;
using HtmlAgilityPack;

namespace RatesParsingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Получить html документ из файла.
            var filePath = @"Other\Sites\myfin.by\myfin.by.html1";
            var inputData = new InputData();
            HtmlDocument HtmlDoc = inputData.GetHtmlDocumentFromFile(filePath);

            // При успешном открытии вывести содержимое документа согласно .
            if (HtmlDoc != null)
                Console.WriteLine(HtmlDoc.Text);
            else
                Console.WriteLine(inputData.ErrorText);
            Console.ReadLine();
        }
    }
}
