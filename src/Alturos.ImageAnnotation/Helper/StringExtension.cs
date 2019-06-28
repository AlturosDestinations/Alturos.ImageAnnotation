using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alturos.ImageAnnotation.Helper
{
    public static class StringExtension
    {
        public static int GetFirstNumber(this string text)
        {
            var tempNumber = new string(
                text.SkipWhile(c => !char.IsDigit(c)).
                TakeWhile(c => char.IsDigit(c)).
                ToArray());

            int.TryParse(tempNumber, out var number);

            return number;
        }

        public static bool Contains(this string str, string value, StringComparison comp)
        {
            return str?.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string ReplaceSpecialCharacters(this string str, char replacementChar = '_')
        {
            var sb = new StringBuilder();

            foreach (var c in str)
            {
                sb.Append(char.IsLetterOrDigit(c) ? c : replacementChar);
            }

            return sb.ToString();
        }
    }
}
