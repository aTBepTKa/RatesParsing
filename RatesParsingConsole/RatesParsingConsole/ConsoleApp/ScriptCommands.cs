﻿using System;
using System.Collections.Generic;
using System.Text;
using RatesParsingConsole.DTO;

namespace RatesParsingConsole.ConsoleApp
{
    /// <summary>
    /// Команды для выполнения парсинга.
    /// </summary>
    class ScriptCommands
    {
        /// <summary>
        /// Получить число из текста.
        /// </summary>
        /// <param name="text">Исходный текст.</param>
        /// <returns></returns>
        public WordProcessingHandler GetNumberFromText()
        {
            string handler(string text)
            {
                string digitText = "";

                foreach (char ch in text)
                {
                    if (char.IsDigit(ch))
                        digitText += ch;
                }
                return digitText;
            }
            return handler;
        }

        /// <summary>
        /// Получить код валюты из конца исходной строки.
        /// </summary>
        /// <param name="length">Длина строки в формате string, так как данные получаем в таком формате.</param>
        /// <returns></returns>
        public WordProcessingHandler GetTextCodeFromEnd(string length)
        {
            if (!int.TryParse(length, out int newLength))
                newLength = 0;

            string handler(string text)
            {
                return text.Substring(text.Length - newLength);
            }
            return handler;
        }

        /// <summary>
        /// Найти и заменить строку в тексте. 
        /// </summary>
        /// <param name="text">Исходный текст.</param>
        /// <param name="oldText">Заменяемая строка.</param>
        /// <param name="newText">Новая строка.</param>
        /// <returns></returns>
        public WordProcessingHandler ReplaceSubstring(string oldText, string newText)
        {
            string handler(string text)
            {
                return text.Replace(oldText, newText);
            }
            return handler;
        }

        /// <summary>
        /// Получить фиксированное значение независимо от входной строки.
        /// </summary>
        /// <param name="value">Значение фиксированного значения.</param>
        /// <returns></returns>
        public WordProcessingHandler GetFixedValue(string value)
        {
            string handler(string text)
            {
                return value;
            }
            return handler;
        }
    }
}
