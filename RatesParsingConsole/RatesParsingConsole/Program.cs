using RatesParsingConsole.AspApp;
using System;
using System.Threading.Tasks;

namespace RatesParsingConsole
{


    class Program
    {
        /// <summary>
        /// Получает курсы обмена валют с различных банков.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Передать управление в с понтом ASP приложение.
            var home = new AspHome();
            home.StartAppAsync();
            Console.WriteLine("Работа программы завершена. Для продолжения нажмите любую клавишу.");
            Console.ReadKey();
        }
    }
}
