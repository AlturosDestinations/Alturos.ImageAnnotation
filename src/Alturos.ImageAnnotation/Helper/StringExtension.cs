using System;
using System.Linq;

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
    }
}
